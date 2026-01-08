using UnityEngine;
using System;

public class ScreenCapture : MonoBehaviour
{
    public Camera mainCamera;
    public RenderTexture renderTexture;

    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;
    }

    public string CaptureScreen()
    {
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        RenderTexture.active = null;

        byte[] bytes = screenShot.EncodeToPNG();

        string base64String = Convert.ToBase64String(bytes);
        return base64String;
    }
}