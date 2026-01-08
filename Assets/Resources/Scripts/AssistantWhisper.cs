using OpenAI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Whisper
{
    public class AssistantWhisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Text message;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private GameObject info;
        [SerializeField] private Button infoButton;
        [SerializeField] private Sprite record, recording;

        private readonly string fileName = "output.wav";
        private readonly int duration = 10;

        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi("sk-proj-LRYIdKJRxgCG5rgNJ5TqT3BlbkFJ8HaYpexi5EdPC5tCIJhC", "org-PnIwYkAigOO5nukpqmqffRLy");
        private AssistantChat chat;
        private AssistantTts tts;

        private void Start()
        {
            chat = FindObjectOfType<AssistantChat>();
            tts = FindObjectOfType<AssistantTts>();
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            infoButton.onClick.AddListener(ShowInfo);

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
            #endif
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        private void StartRecording()
        {
            if (isRecording)
            {
                EndRecording();
            }
            else
            {
                recordButton.image.sprite = recording;
                isRecording = true;
                var index = PlayerPrefs.GetInt("user-mic-device-index");
                #if !UNITY_WEBGL
                clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
                #endif
            }
        }

        private async void EndRecording()
        {
            recordButton.enabled = false;
            isRecording = false;
            recordButton.image.sprite = record;
            message.text = "Transcripting...";

            Microphone.End(dropdown.options[PlayerPrefs.GetInt("user-mic-device-index")
                ].text);

            byte[] data = SaveWav.Save(fileName, clip);

            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            message.text = res.Text;
            string chatRes = await chat.SendReply(res.Text);
            tts.SendRequest(chatRes);
            recordButton.enabled = true;
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;

                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }

        private void ShowInfo()
        {
            info.SetActive(!info.activeSelf);
        }
    }
}
