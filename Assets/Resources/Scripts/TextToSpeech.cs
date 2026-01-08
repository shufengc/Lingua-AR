using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public async Task SpeakText(string textToSpeak, string lang_code)
    {
        var credentials = new BasicAWSCredentials("AKIASH5LD2NA3KVBCU4I", "//Tdw/AFQ4I0+Z4q3fHuMXizidhtPxImqEbIqFOp");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.USEast1);
        VoiceId voice_id;
        switch (lang_code)
        {
            case "Es:":
                voice_id = VoiceId.Lucia;
                break;
            case "Fr:":
                voice_id = VoiceId.Lea;
                break;
            case "De:":
                voice_id = VoiceId.Vicki;
                break;
            case "Zh:":
                voice_id = VoiceId.Zhiyu;
                break;
            default:
                voice_id = VoiceId.Joanna;
                break;
        }

        var request = new SynthesizeSpeechRequest()
        {
            Text = textToSpeak,  // This line replaces the hardcoded text
            Engine = Engine.Neural,
            VoiceId = voice_id,
            OutputFormat = OutputFormat.Mp3,
        };

        var response = await client.SynthesizeSpeechAsync(request);
        WriteIntoFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"file://{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Write Audio to File
    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
            stream.CopyTo(fileStream);
        }
    }

    public async void OnPlayAudioClicked()
    {
        GameObject O_translatedText = GameObject.Find("/Canvas/TranslationInfo/InfoPanel/TextTranslatedText");
        GameObject O_translatedLang = GameObject.Find("/Canvas/TranslationInfo/InfoPanel/TextTranslatedLang");
        string lang_code = O_translatedLang.GetComponent<Text>().text;
        string text_to_speak = O_translatedText.GetComponent<Text>().text;
        Task speak_task = SpeakText(text_to_speak, lang_code);
        await speak_task;
    }

}
