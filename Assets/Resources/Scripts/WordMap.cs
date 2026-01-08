using System;
using System.Collections.Generic;

public class LanguageDictionary
{
    static Dictionary<string, string> ToEnglish;
    static Dictionary<string, string> ToSpanish;
    static Dictionary<string, string> ToFrench;
    static Dictionary<string, string> ToGerman;
    static Dictionary<string, string> ToChinese;

    static private LanguageDictionary ld = new LanguageDictionary();

    public LanguageDictionary()
    {
        // Initialize the dictionary
        ToEnglish = new Dictionary<string, string>();

        // Add some words and their translations
        ToEnglish = new Dictionary<string, string>
        {
            // Add some words and their translations

            // spanish, french, german, chinese

            { "monitor de televisión", "tv monitor" },
            { "moniteur de télévision", "tv monitor"},
            { "TV-Monitor", "tv monitor"},
            { "电视监视器", "tv monitor"},
            
            { "monitor", "tv monitor" },
            { "moniteur", "tv monitor"},
            { "Monitor", "tv monitor"},
            { "显示器", "tv monitor"},


            { "silla", "chair"},
            { "chaise", "chair"},
            { "Stuhl", "chair"},
            { "椅子", "chair"},

            { "teclado", "keyboard"},
            { "clavier", "keyboard"},
            { "Tastatur", "keyboard"},
            { "键盘", "keyboard"},

            { "computadora portátil", "laptop"},
            { "ordinateur portable", "laptop"},
            { "Laptop", "laptop"},
            { "笔记本电脑", "laptop" },

            { "libro", "book"},
            { "livre", "book"},
            { "Buch", "book"},
            { "书", "book"},

            { "botella", "bottle"},
            { "bouteille", "bottle"},
            { "Flasche", "bottle"},
            { "瓶子", "bottle"},

            { "Teléfono móvil", "cell phone"},
            { "téléphone portable", "cell phone"},
            { "Handy", "cell phone"},
            { "手机", "cell phone"},

            { "teléfono", "cell phone"},
            { "téléphone", "cell phone"},
            { "Telefon", "cell phone"},
            { "电话", "cell phone"},

            { "ratón", "mouse"},
            { "souris", "mouse"},
            { "Maus", "mouse"},
            { "鼠标", "mouse"},

            { "manzana", "apple"},
            { "pomme", "apple"},
            { "Apfel", "apple"},
            { "苹果", "apple"}
        };

        ToSpanish = new Dictionary<string, string>();

        ToSpanish = new Dictionary<string, string>
        {

            { "tv monitor", "monitor de televisión" },

            { "chair", "silla"},

            { "keyboard", "teclado"},

            { "laptop", "computadora portátil"},

            { "book", "libro"},

            { "bottle", "botella"},

            { "cell phone", "Teléfono móvil"},

            { "mouse", "ratón"},

            { "apple", "manzana"},
        };

        ToFrench = new Dictionary<string, string>();

        ToFrench = new Dictionary<string, string>
        {

            { "tv monitor", "moniteur de télévision" },

            { "chair", "chaise"},

            { "keyboard", "clavier"},

            { "laptop", "ordinateur portable"},

            { "book", "livre"},

            { "bottle", "bouteille"},

            { "cell phone", "téléphone portable"},

            { "mouse", "souris"},

            { "apple", "pomme"},
        };

        ToGerman = new Dictionary<string, string>();

        ToGerman = new Dictionary<string, string>
        {
            { "tv monitor", "TV-Monitor"},

            { "chair", "Stuhl"},

            { "keyboard", "Tastatur"},

            { "laptop", "Laptop"},

            { "book", "Buch"},

            { "bottle", "Flasche"},

            { "cell phone", "Handy"},

            { "mouse", "Maus"},

            { "apple", "Apfel"},
        };

        ToChinese = new Dictionary<string, string>();

        ToChinese = new Dictionary<string, string>
        {
            { "tv monitor", "电视监视器"},

            { "chair", "椅子"},

            { "keyboard", "键盘"},

            { "laptop", "笔记本电脑"},

            { "book", "书"},

            { "bottle", "瓶子"},

            { "cell phone", "手机"},

            { "mouse", "鼠标"},

            { "apple", "苹果"}
        };
    }

       

    public static string TranslateToEnglish(string ForeignWord)
    {
        // Check if the word exists in the dictionary
        if (ToEnglish.ContainsKey(ForeignWord))
        {
            return ToEnglish[ForeignWord];
        }
        else
        {
            return "Translation not found";
        }
    }

    public static string TranslateToOther(string EnglishWord, string language_type)
    {
        switch (language_type)
        {
            case "es":
                return ToSpanish[EnglishWord];
            case "fr":
                return ToFrench[EnglishWord];
            case "de":
                return ToGerman[EnglishWord];
            case "zh":
                return ToChinese[EnglishWord];
            default:
                return ToSpanish[EnglishWord];
        }

    }
}
