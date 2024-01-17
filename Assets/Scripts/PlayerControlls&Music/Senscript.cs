using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Senscript : MonoBehaviour
{
    public Slider senseSlider;
    public float maxSensetivity;
    public float minSensetivity;
    public static float sense;
    // Start is called before the first frame update
    void Start()
    {
        senseSlider.onValueChanged.AddListener(ChangeMouseSense);
    }

    void ChangeMouseSense(float input)
    {
        sense = input * (maxSensetivity - minSensetivity) + minSensetivity;
        PlayerPrefs.SetFloat("sensitivity", input * (maxSensetivity - minSensetivity) + minSensetivity);
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
