using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    public Slider slider;
    public float maxValue;
    public float minValue;
    public string valueName;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(UpdateValue);
    }


    void UpdateValue(float input)
    {
        PlayerPrefs.SetFloat(valueName, input * (maxValue - minValue) + minValue); 
    }
}
