using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComissionController : MonoBehaviour
{
    public string[] names;//names of people for comission text
    public string[] toyTypes;//types of toys that can be requested 
    public string[] colors;//Color you want for the toy
    public Vector3[] colorRGBValues;
    public string[] flavorText;

    public string requestText;
    public int numberOfComissions;
    public Text currectCommission;


    void Start()
    {
        numberOfComissions = 0;
        SetNewComission();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewComission()
    {
        numberOfComissions++;
        int[] inputNums = new int[4];//random name of person, type of toy, and color, can add more perameters
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                inputNums[i] = Random.Range(0, names.Length);
            }
            else if (i == 1)
            {
                inputNums[i] = Random.Range(0, toyTypes.Length);
            }
            else if (i == 2)
            {
                inputNums[i] = Random.Range(0, colors.Length);
            }
            else if (i == 3)
            {
                inputNums[i] = Random.Range(0, flavorText.Length);
            }
            Debug.Log(inputNums[i]);
        }

        int sentenceStructureRand = Random.Range(0, 5);
        sentenceStructureRand = 0;//for testing
        if (sentenceStructureRand == 0)
        {
            requestText = "Hey I'm " + names[inputNums[0]] + ". " + flavorText[inputNums[3]] + " I want a " + toyTypes[inputNums[1]] + " in the color " + colors[inputNums[2]] + ".";
            Debug.Log(requestText);
        }
        else if (sentenceStructureRand == 1)
        {
            //requestText = ;
        }
        else if (sentenceStructureRand == 2)
        {
            //requestText = ;
        }
        else if (sentenceStructureRand == 3)
        {
            //requestText = ;
        }
        else if (sentenceStructureRand == 4)
        {
            //requestText = ;
        }
        currectCommission.text = names[inputNums[0]] + " requested a " + colors[inputNums[2]] + ", " + toyTypes[inputNums[1]] + ".";
    }

    public void RemoveOldComission()
    {
        numberOfComissions--;
    }
}
