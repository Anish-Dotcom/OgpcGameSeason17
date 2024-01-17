using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderScript : MonoBehaviour
{

    public Slider slider;
    public float maxValue;
    public float minValue;
    public float displayMaxValue = 100;
    public float displayMinValue = 0;
    public string valueName;
    public string header;
    public float defualtValue;
    public TMP_Text label;
    // Start is called before the first frame update
    void Start()
    {

        slider.onValueChanged.AddListener(UpdateValue);
        if (!PlayerPrefs.HasKey(valueName))
        {
            PlayerPrefs.SetFloat(valueName, (defualtValue - displayMinValue) / displayMaxValue * ((maxValue - minValue) + minValue));
        }

        slider.value = (PlayerPrefs.GetFloat(valueName) - minValue) / (maxValue - minValue);
    }


    void UpdateValue(float input)
    {
        float value = input * (maxValue - minValue) + minValue;
        PlayerPrefs.SetFloat(valueName, value);
        label.text = header + Math.Round((value / maxValue * (displayMaxValue - displayMinValue)) + displayMinValue);
    }
}
