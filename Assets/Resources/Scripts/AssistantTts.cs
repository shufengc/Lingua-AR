using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    [RequireComponent(typeof(AudioSource))]
    public class AssistantTts : MonoBehaviour
    {
        [SerializeField] private Dropdown voiceDropdown;
        [SerializeField] private Dropdown modelDropdown;
        [SerializeField] private Button playButton;

        private AudioSource audioSource;

        private OpenAIApi openai = new OpenAIApi("sk-proj-LRYIdKJRxgCG5rgNJ5TqT3BlbkFJ8HaYpexi5EdPC5tCIJhC", "org-PnIwYkAigOO5nukpqmqffRLy");

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            playButton.onClick.AddListener(Play);
        }

        public async void SendRequest(string text)
        {
            var request = new CreateTextToSpeechRequest
            {
                Input = text,
                Model = modelDropdown.options[modelDropdown.value].text.ToLower(),
                Voice = voiceDropdown.options[voiceDropdown.value].text.ToLower()
            };


            var response = await openai.CreateTextToSpeech(request);

            if (response.AudioClip)
            {
                audioSource.clip = response.AudioClip;
                audioSource.Play();
            }
        }
        public void Play()
        {
            audioSource.Play();

        }
    }
}
