using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using TMPro.EditorUtilities;
using UnityEngine.EventSystems;

public class SettingsKeybind : MonoBehaviour
{
    public string valueName;
    public TMP_Text displayedText;
    public TMP_InputField inputField;
    public string defualtValue;
    // Start is called before the first frame update
    void Start()
    {
        inputField.caretWidth = 0;
        if (!PlayerPrefs.HasKey(valueName))
        {
            PlayerPrefs.SetString(valueName, defualtValue);
        }

        inputField.text = PlayerPrefs.GetString(valueName);


        Color selectionColor = inputField.selectionColor;
        selectionColor.a = 0;
        inputField.selectionColor = selectionColor;

        inputField.onValueChanged.AddListener(updateValue);
    }

    void updateValue(string input)
    {
        inputField.text = inputField.text.ToCharArray()[^1].ToString();
        if (inputField.text == " ")
        {
            inputField.text = "_";
            PlayerPrefs.SetString(valueName, "Space");
        } else
        {
            PlayerPrefs.SetString(valueName, inputField.text);
        }  
    }
    void Update()
    {

        inputField.caretPosition = inputField.text.Length;
    }

}
