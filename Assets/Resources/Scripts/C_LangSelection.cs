using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_LangSelection : MonoBehaviour
{
    public void Start()
    {
        LanguageSelection.Set_language("es");
    }

    public void Set_Language()
    {
        string selected_lang;

        switch (gameObject.GetComponent<Dropdown>().value)
        {
            case 0:
                selected_lang = "es";
                break;
            case 1:
                selected_lang = "fr";
                break;
            case 2:
                selected_lang = "de";
                break;
            case 3:
                selected_lang = "zh";
                break;
            default:
                selected_lang = "es";
                break;
        }

        LanguageSelection.Set_language(selected_lang);
    }
}
