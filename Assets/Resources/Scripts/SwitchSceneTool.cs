using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SwitchSceneTool : MonoBehaviour
{    
    // Start is called before the first frame update

    public void SwitchToGame()
    {
        Debug.Log("To Game");

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

        SceneManager.LoadScene("VocabGame");
    }

    public void SwitchToMain()
    {
        Debug.Log("To Main");
        SceneManager.LoadScene("MainScene");
    }
}
