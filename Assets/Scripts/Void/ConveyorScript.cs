using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    private SellCalculation sellCalculation;
    // Start is called before the first frame update
    void Start()
    {
        sellCalculation = transform.parent.GetComponent<SellCalculation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
