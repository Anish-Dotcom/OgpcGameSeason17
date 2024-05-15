using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swiperController : MonoBehaviour
{
    public MainRadarScript MainRadarScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("detected");
        if (collision.tag == "Dot")
        {
            print("detected");
        }
    }
}
