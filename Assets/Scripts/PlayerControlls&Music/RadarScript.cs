using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadarScript : MonoBehaviour
{
    public bool objectInRange;
    public float distToObject;
    public Image Image;
    public Sprite DefaultRadar;
    public Sprite Radar1;
    public Sprite Radar2;
    public Sprite Radar3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectInRange)
        {
            if(distToObject < 1)
            {
                Image.sprite = Radar1;
            }
            else if (distToObject < 2)
            {
                Image.sprite = Radar2;
            }
            else if (distToObject < 3)
            {
                Image.sprite = Radar3;
            } else
            {
                Image.sprite = DefaultRadar;
            }
        }
    }
}
