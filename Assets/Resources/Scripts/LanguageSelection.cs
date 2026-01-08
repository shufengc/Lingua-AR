using UnityEngine;

public static class LanguageSelection
{
    static private string SelectedLanguage;

    static public void Set_language(string selected_language)
    {
        LanguageSelection.SelectedLanguage = selected_language;
        Debug.Log("Set Language: " + LanguageSelection.SelectedLanguage);
    }

    static public string Get_Language()
    {
        Debug.Log("Get Language: " + LanguageSelection.SelectedLanguage);
        return LanguageSelection.SelectedLanguage;
    }
}
