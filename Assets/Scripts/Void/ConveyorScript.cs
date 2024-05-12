using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    private SellCalculation sellCalculation;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        sellCalculation = transform.parent.GetComponent<SellCalculation>();
        player = sellCalculation.player;
    }

    private void Update()
    {
        
    }
    public void Sellitem()
    {
        //sellCalculation.CalculatePrice(player, );
    }
}
