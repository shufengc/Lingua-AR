using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_SetLang : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string display_targeted_lang;

        switch (LanguageSelection.Get_Language())
        {
            case "es":
                display_targeted_lang = "Es:";
                break;
            case "fr":
                display_targeted_lang = "Fr:";
                break;
            case "de":
                display_targeted_lang = "De:";
                break;
            case "zh":
                display_targeted_lang = "Zh:";
                break;
            default:
                display_targeted_lang = "Es:";
                break;
        }

        gameObject.GetComponent<Text>().text = display_targeted_lang;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
