using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace OpenCVForUnityExample
{
    public class OpenCVForUnityExample : MonoBehaviour
    {
        public Text versionInfo;
        public ScrollRect scrollRect;
        static float verticalNormalizedPosition = 1f;

        // Use this for initialization
        void Start()
        {
            versionInfo.text = Core.NATIVE_LIBRARY_NAME + " " + Utils.getVersion() + " (" + Core.VERSION + ")";
            versionInfo.text += " / UnityEditor " + Application.unityVersion;
            versionInfo.text += " / ";

#if UNITY_EDITOR
            versionInfo.text += "Editor";
#elif UNITY_STANDALONE_WIN
            versionInfo.text += "Windows";
#elif UNITY_STANDALONE_OSX
            versionInfo.text += "Mac OSX";
#elif UNITY_STANDALONE_LINUX
            versionInfo.text += "Linux";
#elif UNITY_ANDROID
            versionInfo.text += "Android";
#elif UNITY_IOS
            versionInfo.text += "iOS";
#elif UNITY_WSA
            versionInfo.text += "WSA";
#elif UNITY_WEBGL
            versionInfo.text += "WebGL";
#endif
            versionInfo.text += " ";
#if ENABLE_MONO
            versionInfo.text += "Mono";
#elif ENABLE_IL2CPP
            versionInfo.text += "IL2CPP";
#elif ENABLE_DOTNET
            versionInfo.text += ".NET";
#endif

            scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;

#if UNITY_WSA_10_0
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/BarcodeDetectorExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/BarcodeDetectorWebCamExampleButton").GetComponent<Button>().interactable = false;

            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FaceDetectorYNWebCamExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FaceRecognizerSFExampleButton").GetComponent<Button>().interactable = false;

            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ColorizationExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectTrackingDaSiamRPNExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FastNeuralStyleTransferExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FaceDetectionResnetSSDExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FaceDetectionYuNetExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FaceDetectionYuNetV2ExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/FacialExpressionRecognitionExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/PoseEstimationMediaPipeExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/HandPoseEstimationMediaPipeExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/HumanSegmentationPPHumanSegExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ImageClassificationMobilenetExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ImageClassificationPPResnetExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectDetectionMobileNetSSDExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectDetectionMobileNetSSDWebCamExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectDetectionYOLOv4ExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectDetectionYOLOXExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/ObjectDetectionNanoDetPlusExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/TextRecognitionCRNNExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/TextRecognitionCRNNWebCamExampleButton").GetComponent<Button>().interactable = false;

            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/ContribModulesGroup/TextDetectionExampleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/ContribModulesGroup/TextRecognitionExampleButton").GetComponent<Button>().interactable = false;
#endif


#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_OSX && !UNITY_LINUX && !UNITY_IOS && !UNITY_ANDROID
            GameObject.Find("Canvas/Panel/SceneList/ScrollView/List/MainModulesGroup/VideoCaptureCameraInputExampleButton").GetComponent<Button>().interactable = false;
#endif
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnScrollRectValueChanged()
        {
            verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
        }


        public void OnShowSystemInfoButtonClick()
        {
            SceneManager.LoadScene("ShowSystemInfo");
        }

        public void OnShowLicenseButtonClick()
        {
            SceneManager.LoadScene("ShowLicense");
        }

        #region Main modules

        #region dnn

        public void OnObjectDetectionYOLOXExampleButtonClick()
        {
            TMP_Dropdown dropdown = GameObject.Find("/Canvas/LanguageSelection").GetComponent<TMP_Dropdown>();

            switch (dropdown.value)
            {
                case 0:
                    LanguageSelection.Set_language("es");
                    break;
                case 1:
                    LanguageSelection.Set_language("fr");
                    break;
                case 2:
                    LanguageSelection.Set_language("de");
                    break;
                case 3:
                    LanguageSelection.Set_language("zh");
                    break;
                default:
                    LanguageSelection.Set_language("es");
                    break;
            }

            SceneManager.LoadScene("ObjectDetection");
        }
        
        #endregion


        #endregion
    }
}