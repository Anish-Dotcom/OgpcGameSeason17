using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    public string valueName;
    public int defualtValue;
    void Start()
    {
        if (!PlayerPrefs.HasKey(valueName))
        {
            PlayerPrefs.SetInt(valueName, defualtValue);
        }

        transform.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt(valueName) == 1;
        transform.GetComponent<Toggle>().onValueChanged.AddListener(updateValue);
    }

    void updateValue(bool input)
    {
        if (input)
        {
            PlayerPrefs.SetInt(valueName, 1);
        } else
        {
            PlayerPrefs.SetInt(valueName, 0);
        }
    }
}
