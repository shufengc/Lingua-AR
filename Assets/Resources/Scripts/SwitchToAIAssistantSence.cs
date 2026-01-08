using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToAIAssistantSence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SwitchToAIAssistant()
    {
        Debug.Log("To AI");

        SceneManager.LoadScene("Assistant_RealScene");
    }

    public void SwitchToMainScene()
    {
        Debug.Log("To main");

        SceneManager.LoadScene("MainScene");
    }
}
