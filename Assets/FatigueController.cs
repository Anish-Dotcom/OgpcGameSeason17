using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public float fatigue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            fatigue++;
            print(fatigue);
            transform.hasChanged = false;
        }

        if(fatigue> 10000) {
            print("tired");
        }

    }
}
