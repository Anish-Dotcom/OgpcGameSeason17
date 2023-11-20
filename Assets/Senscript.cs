using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Senscript : MonoBehaviour
{
    public Slider senseSlider;
    public float senseMult;

    // Start is called before the first frame update
    void Start()
    {
        senseSlider.onValueChanged.AddListener(ChngMouseSense);
    }

    void ChngMouseSense(float input)
    {
        float sense = input * senseMult;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
