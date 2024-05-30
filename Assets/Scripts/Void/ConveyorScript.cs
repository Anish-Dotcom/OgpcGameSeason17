using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConveyorScript : MonoBehaviour
{
    private ComissionController comissionController;
    private SellCalculation sellCalculation;
    private GameObject player;
    private PopupInfo popupInfo;
    // Start is called before the first frame update
    void Start()
    {
        sellCalculation = transform.parent.GetComponent<SellCalculation>();
        comissionController = transform.parent.GetComponent<ComissionController>();
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
        GameObject objToSell = player.GetComponent<ObjectPickUp>().currentObject.transform.parent.gameObject;
        Debug.Log(objToSell);
        if (objToSell.name.Contains("finished Toy"))
        {
            int comissionCompleting = 1;/*
            for (int i = 0; i < comissionController.numberOfComissions; i++)
            {
                int inputNum = comissionController.inputNums[4 * i];
                Debug.Log("Name " + comissionController.names[inputNum] + "Text " + tagCanvas.text);
                if (comissionController.names[inputNum] == tagCanvas.text)
                {
                    comissionCompleting = i;
                }
            }*/
            if (comissionCompleting != -1)
            {
                sellCalculation.CalculatePrice(objToSell, comissionCompleting);
            }
        }
    }
}