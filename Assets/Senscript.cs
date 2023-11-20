using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Senscript : MonoBehaviour
{
    public Slider senseSlider;
    public float senseMult;
    public static float sense;
    // Start is called before the first frame update
    void Start()
    {
        senseSlider.onValueChanged.AddListener(ChngMouseSense);
    }

    void ChngMouseSense(float input)
    {
        sense = input * senseMult;

        Debug.Log(sense);
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
