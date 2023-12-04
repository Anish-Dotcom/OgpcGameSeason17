using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    public string valueName;
    // Start is called before the first frame update
    void Start()
    {
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
