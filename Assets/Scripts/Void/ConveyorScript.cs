using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    private SellCalculation sellCalculation;
    private GameObject player;
    private PopupInfo popupInfo;
    // Start is called before the first frame update
    void Start()
    {
        sellCalculation = transform.parent.GetComponent<SellCalculation>();
        player = sellCalculation.player;
        popupInfo = GetComponent<PopupInfo>();
    }

    private void Update()
    {
        if (popupInfo.lookingAt[0])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Sellitem();
            }
        }
    }
    public void Sellitem()
    {
        GameObject objToSell = player.GetComponent<ObjectPickUp>().currentObject.transform.GetChild(0).gameObject;
        if (objToSell.name.Contains("sellbox"))
        {
            //sellCalculation.CalculatePrice(objToSell.transform.GetChild(0).gameObject, );
        }
    }
}