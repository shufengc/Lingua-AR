using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetInputText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText()
    {
        string inputString = GameObject.Find("Hint").GetComponent<Text>().text;
        string[] parts = inputString.Split(new string[] { "\n" }, StringSplitOptions.None);

        Debug.Log("parts " + parts[1]);
        // Check if there are at least two parts (before and after the newline)
        if (parts.Length >= 2)
        {
            Debug.Log("ready to set");
            GameObject.Find("InputWord").GetComponent<InputField>().text = parts[1];
        }
    }
}
