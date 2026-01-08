#if !UNITY_WSA_10_0

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnityExample.DnnModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;


namespace OpenCVForUnityExample
{
    public class DetectRst
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        public string labelName;
        public string translatedName;

        public DetectRst(float _left, float _top, float _right, float _bottom, string _labelName)
        {
            left = _left;
            top = _top;
            right = _right;
            bottom = _bottom;
            labelName = _labelName;
        }

        public void UpdateLabelName(string newLabelName)
        {
            translatedName = newLabelName;
        }
    }



    /// <summary>
    /// Object Detection YOLOX Example
    /// An example of using OpenCV dnn module with YOLOX Object Detection.
    /// Referring to https://github.com/opencv/opencv_zoo/tree/master/models/object_detection_yolox
    /// https://github.com/Megvii-BaseDetection/YOLOX
    /// https://github.com/Megvii-BaseDetection/YOLOX/tree/main/demo/ONNXRuntime
    /// 
    /// [Tested Models]
    /// yolox_nano.onnx https://github.com/Megvii-BaseDetection/YOLOX/releases/download/0.1.1rc0/yolox_nano.onnx
    /// yolox_tiny.onnx https://github.com/Megvii-BaseDetection/YOLOX/releases/download/0.1.1rc0/yolox_tiny.onnx
    /// yolox_s.onnx https://github.com/Megvii-BaseDetection/YOLOX/releases/download/0.1.1rc0/yolox_s.onnx
    /// </summary>
    [RequireComponent(typeof(WebCamTextureToMatHelper))]
    public class ObjectDetectionYOLOXExample : MonoBehaviour
    {
        [TooltipAttribute("Path to a binary file of model contains trained weights. It could be a file with extensions .caffemodel (Caffe), .pb (TensorFlow), .t7 or .net (Torch), .weights (Darknet).")]
        public string model = "yolox_tiny.onnx";

        [TooltipAttribute("Path to a text file of model contains network configuration. It could be a file with extensions .prototxt (Caffe), .pbtxt (TensorFlow), .cfg (Darknet).")]
        public string config = "";

        [TooltipAttribute("Optional path to a text file with names of classes to label detected objects.")]
        public string classes = "coco.names";

        [TooltipAttribute("Confidence threshold.")]
        public float confThreshold = 0.25f;

        [TooltipAttribute("Non-maximum suppression threshold.")]
        public float nmsThreshold = 0.45f;

        [TooltipAttribute("Maximum detections per image.")]
        public int topK = 1000;

        [TooltipAttribute("Preprocess input image by resizing to a specific width.")]
        public int inpWidth = 416;

        [TooltipAttribute("Preprocess input image by resizing to a specific height.")]
        public int inpHeight = 416;

        public GameObject O_TranslationInfo;
        public GameObject O_AudioSource;


        [Header("TEST")]

        [TooltipAttribute("Path to test input image.")]
        public string testInputImage;

        protected string classes_filepath;
        protected string config_filepath;
        protected string model_filepath;


        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The webcam texture to mat helper.
        /// </summary>
        WebCamTextureToMatHelper webCamTextureToMatHelper;

        /// <summary>
        /// The bgr mat.
        /// </summary>
        Mat bgrMat;

        /// <summary>
        /// The YOLOX ObjectDetector.
        /// </summary>
        YOLOXObjectDetector objectDetector;

        /// <summary>
        /// The FPS monitor.
        /// </summary>

        Mat static_rgbaMat;
        Mat static_result;

        private string language;

        public List<DetectRst> detectObj = new List<DetectRst>();

        bool have_updated_label = false;

        [Serializable]
        public class TranslationResponse
        {
            public TranslationData data;
        }

        [Serializable]
        public class TranslationData
        {
            public Translation[] translations;
        }

        [Serializable]
        public class Translation
        {
            public string translatedText;
            public string detectedSourceLanguage;
        }
        private readonly string apiKey = "AIzaSyDYJ4ZOszr5A4q3d_VlgcB5V6MNTboLokk";
        private readonly string url = "https://translation.googleapis.com/language/translate/v2";


#if UNITY_WEBGL
        IEnumerator getFilePath_Coroutine;
#endif

        // Use this for initialization
        void Start()
        {
            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();

#if UNITY_WEBGL
            getFilePath_Coroutine = GetFilePath();
            StartCoroutine(getFilePath_Coroutine);
#else
            if (!string.IsNullOrEmpty(classes))
            {
                classes_filepath = Utils.getFilePath("OpenCVForUnity/dnn/" + classes);
                if (string.IsNullOrEmpty(classes_filepath)) Debug.Log("The file:" + classes + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }
            if (!string.IsNullOrEmpty(config))
            {
                config_filepath = Utils.getFilePath("OpenCVForUnity/dnn/" + config);
                if (string.IsNullOrEmpty(config_filepath)) Debug.Log("The file:" + config + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }
            if (!string.IsNullOrEmpty(model))
            {
                model_filepath = Utils.getFilePath("OpenCVForUnity/dnn/" + model);
                if (string.IsNullOrEmpty(model_filepath)) Debug.Log("The file:" + model + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }

            Run();
#endif
        }

#if UNITY_WEBGL
        private IEnumerator GetFilePath()
        {
            if (!string.IsNullOrEmpty(classes))
            {
                var getFilePathAsync_0_Coroutine = Utils.getFilePathAsync("OpenCVForUnity/dnn/" + classes, (result) =>
                {
                    classes_filepath = result;
                });
                yield return getFilePathAsync_0_Coroutine;

                if (string.IsNullOrEmpty(classes_filepath)) Debug.Log("The file:" + classes + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }

            if (!string.IsNullOrEmpty(config))
            {
                var getFilePathAsync_1_Coroutine = Utils.getFilePathAsync("OpenCVForUnity/dnn/" + config, (result) =>
                {
                    config_filepath = result;
                });
                yield return getFilePathAsync_1_Coroutine;

                if (string.IsNullOrEmpty(config_filepath)) Debug.Log("The file:" + config + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }

            if (!string.IsNullOrEmpty(model))
            {
                var getFilePathAsync_2_Coroutine = Utils.getFilePathAsync("OpenCVForUnity/dnn/" + model, (result) =>
                {
                    model_filepath = result;
                });
                yield return getFilePathAsync_2_Coroutine;

                if (string.IsNullOrEmpty(model_filepath)) Debug.Log("The file:" + model + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");
            }

            getFilePath_Coroutine = null;

            Run();
        }
#endif

        // Call Cloud Translation API to realize TranslateText
        public IEnumerator TranslateText(string textToTranslate, String targetLanguage, System.Action<string> callback)
        {
            string jsonRequestBody = $"{{'q': '{textToTranslate}', 'target': '{targetLanguage}', 'format': 'text'}}";
            string completeUrl = $"{url}?key={apiKey}";
            Debug.Log("JsonRequestBody: " + jsonRequestBody);

            using (UnityWebRequest request = UnityWebRequest.Post(completeUrl, jsonRequestBody))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.uploadHandler.contentType = "application/json";
                request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonRequestBody));

                yield return request.SendWebRequest();

                while (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Translation error: " + request.error);
                    yield return null;
                }

                string jsonResponse = request.downloadHandler.text;
                string translatedText = ExtractTranslatedTextFromJson(jsonResponse);
                Debug.Log("SB" + translatedText);
                callback(translatedText);
            }
        }

        private string ExtractTranslatedTextFromJson(string json)
        {
            TranslationResponse response = JsonConvert.DeserializeObject<TranslationResponse>(json);
            return response.data.translations[0].translatedText;
        }

        // Use this for initialization
        void Run()
        {
            //if true, The error log of the Native side OpenCV will be displayed on the Unity Editor Console.
            Utils.setDebugMode(true);


            if (string.IsNullOrEmpty(model_filepath) || string.IsNullOrEmpty(classes_filepath))
            {
                Debug.LogError("model: " + model + " or " + "config: " + config + " or " + "classes: " + classes + " is not loaded.");
            }
            else
            {
                objectDetector = new YOLOXObjectDetector(model_filepath, config_filepath, classes_filepath, new Size(inpWidth, inpHeight), confThreshold, nmsThreshold, topK);
            }

            if (string.IsNullOrEmpty(testInputImage))
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
                webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
#endif
                webCamTextureToMatHelper.Initialize();
            }
            else
            {
                /////////////////////
                // TEST

                var getFilePathAsync_0_Coroutine = Utils.getFilePathAsync("OpenCVForUnity/dnn/" + testInputImage, (result) =>
                {
                    string test_input_image_filepath = result;
                    if (string.IsNullOrEmpty(test_input_image_filepath)) Debug.Log("The file:" + testInputImage + " did not exist in the folder “Assets/StreamingAssets/OpenCVForUnity/dnn”.");

                    Mat img = Imgcodecs.imread(test_input_image_filepath);
                    if (img.empty())
                    {
                        img = new Mat(424, 640, CvType.CV_8UC3, new Scalar(0, 0, 0));
                        Imgproc.putText(img, testInputImage + " is not loaded.", new Point(5, img.rows() - 30), Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                        Imgproc.putText(img, "Please read console message.", new Point(5, img.rows() - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                    }
                    else
                    {
                        TickMeter tm = new TickMeter();
                        tm.start();

                        Mat results = objectDetector.infer(img);

                        tm.stop();
                        Debug.Log("YOLOXObjectDetector Inference time (preprocess + infer + postprocess), ms: " + tm.getTimeMilli());

                        objectDetector.visualize(ref img, results, true, false);
                    }

                    gameObject.transform.localScale = new Vector3(img.width(), img.height(), 1);
                    float imageWidth = img.width();
                    float imageHeight = img.height();
                    float widthScale = (float)Screen.width / imageWidth;
                    float heightScale = (float)Screen.height / imageHeight;
                    if (widthScale < heightScale)
                    {
                        Camera.main.orthographicSize = (imageWidth * (float)Screen.height / (float)Screen.width) / 2;
                    }
                    else
                    {
                        Camera.main.orthographicSize = imageHeight / 2;
                    }

                    Imgproc.cvtColor(img, img, Imgproc.COLOR_BGR2RGB);
                    Texture2D texture = new Texture2D(img.cols(), img.rows(), TextureFormat.RGB24, false);
                    Utils.matToTexture2D(img, texture);
                    gameObject.GetComponent<Renderer>().material.mainTexture = texture;

                });
                StartCoroutine(getFilePathAsync_0_Coroutine);

                /////////////////////
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper initialized event.
        /// </summary>
        public void OnWebCamTextureToMatHelperInitialized()
        {
            Debug.Log("OnWebCamTextureToMatHelperInitialized");

            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

            texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(webCamTextureMat, texture);

            gameObject.GetComponent<Renderer>().material.mainTexture = texture;

            gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);
            Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);


            float width = webCamTextureMat.width();
            float height = webCamTextureMat.height();

            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;
            if (widthScale < heightScale)
            {
                Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            }
            else
            {
                Camera.main.orthographicSize = height / 2;
            }

            bgrMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC3);
        }

        /// <summary>
        /// Raises the webcam texture to mat helper disposed event.
        /// </summary>
        public void OnWebCamTextureToMatHelperDisposed()
        {
            Debug.Log("OnWebCamTextureToMatHelperDisposed");

            if (bgrMat != null)
                bgrMat.Dispose();

            if (texture != null)
            {
                Texture2D.Destroy(texture);
                texture = null;
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }


        // Error message Logic


        // Update is called once per frame
        void Update()
        {
            if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Button");
                foreach (GameObject go in gos)
                    Destroy(go);

                Mat rgbaMat = webCamTextureToMatHelper.GetMat();
                static_rgbaMat = rgbaMat.clone();

                if (objectDetector == null)
                {
                    Imgproc.putText(rgbaMat, "Model file is not loaded.", new Point(5, rgbaMat.rows() - 30),
                                    Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2,
                                    Imgproc.LINE_AA, false);
                    Imgproc.putText(rgbaMat, "Please read console message.", new Point(5, rgbaMat.rows() - 10),
                                    Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2,
                                    Imgproc.LINE_AA, false);
                }
                else
                {
                    Imgproc.cvtColor(rgbaMat, bgrMat, Imgproc.COLOR_RGBA2BGR);

                    // Object detection logic
                    Mat results = objectDetector.infer(bgrMat);

                    static_result = results;

                    Imgproc.cvtColor(bgrMat, rgbaMat, Imgproc.COLOR_BGR2RGBA);

                    objectDetector.visualize(ref rgbaMat, results, false, true);
                }

                Utils.matToTexture2D(rgbaMat, texture);
                have_updated_label = false;
            }
            else
            {
                if (!have_updated_label)
                {
                    Mat rgbaMat = static_rgbaMat.clone();

                    Imgproc.cvtColor(rgbaMat, bgrMat, Imgproc.COLOR_RGBA2BGR);
                    Mat results = objectDetector.infer(bgrMat);

                    if (objectDetector == null)
                    {
                        Imgproc.putText(rgbaMat, "Model file is not loaded.", new Point(5, rgbaMat.rows() - 30),
                                        Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2,
                                        Imgproc.LINE_AA, false);
                        Imgproc.putText(rgbaMat, "Please read console message.", new Point(5, rgbaMat.rows() - 10),
                                        Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2,
                                        Imgproc.LINE_AA, false);
                    }
                    else
                    {
                        if (!static_result.empty())
                        {
                            Debug.Log("Result Not empty");
                        }
                        load_detection_rst(static_result);
                    }
                    // Re-visualize with the updated labels
                    objectDetector.visualize(ref rgbaMat, results, false, true, true, O_TranslationInfo, O_AudioSource.GetComponent<AudioSource>());

                    Utils.matToTexture2D(rgbaMat, texture);

                    have_updated_label = true;
                }
            }
        }

        void load_detection_rst(Mat results)
        {
            if (results.empty())
            {
                Debug.Log("Result Empty!");
                return;
            }

            if (results.cols() < 6)
            {
                Debug.Log("Result Col" + results.cols().ToString());
                return;
            }
                

            detectObj.Clear();

            List<string> classNames = objectDetector.get_classNames();

            Debug.Log("Result Rows:" + results.rows().ToString());

            for (int i = results.rows() - 1; i >= 0; --i)
            {
                float[] box = new float[4];
                results.get(i, 0, box);
                float[] conf = new float[1];
                results.get(i, 4, conf);
                float[] cls = new float[1];
                results.get(i, 5, cls);

                float left = box[0];
                float top = box[1];
                float right = box[2];
                float bottom = box[3];
                int classId = (int)cls[0];

                DetectRst ObjInfo = new DetectRst(left, top, right, bottom, classNames[classId]);
                detectObj.Add(ObjInfo);
                Debug.Log("detectObj");
            }
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        void OnDestroy()
        {
            webCamTextureToMatHelper.Dispose();

            if (objectDetector != null)
                objectDetector.dispose();

            Utils.setDebugMode(false);

#if UNITY_WEBGL
            if (getFilePath_Coroutine != null)
            {
                StopCoroutine(getFilePath_Coroutine);
                ((IDisposable)getFilePath_Coroutine).Dispose();
            }
#endif
        }

        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnBackButtonClick()
        {
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            webCamTextureToMatHelper.Play();
            objectDetector.buttons.Clear();
        }

        /// <summary>
        /// Raises the pause button click event.
        /// </summary>
        public void OnPauseButtonClick()
        {
            webCamTextureToMatHelper.Pause();
        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnStopButtonClick()
        {
            webCamTextureToMatHelper.Stop();
        }

        /// <summary>
        /// Raises the change camera button click event.
        /// </summary>
        public void OnChangeCameraButtonClick()
        {
            webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.requestedIsFrontFacing;
        }

        public void SetButtonsActive()
        {
            foreach (GameObject button in objectDetector.buttons)
            {
                button.SetActive(true);
            }
        }

        public void Translate()
        {
            string targeted_lang = LanguageSelection.Get_Language();

            GameObject O_translatedText = GameObject.Find("/Canvas/TranslationInfo/InfoPanel/TextTranslatedText");
            GameObject O_originalText = GameObject.Find("/Canvas/TranslationInfo/InfoPanel/TextOriginalText");

            string original_text = O_originalText.GetComponent<Text>().text;

            StartCoroutine(TranslateText(original_text, targeted_lang, translatedLabel =>
            {
                Debug.Log("Label Updated:"+ translatedLabel);
                O_translatedText.GetComponent<Text>().text = translatedLabel;
            }));
        }
    }
}

#endif