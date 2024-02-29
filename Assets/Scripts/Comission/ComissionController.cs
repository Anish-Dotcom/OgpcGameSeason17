using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComissionController : MonoBehaviour
{
    public string[] variableNames;//names of each of the perameters

    public string[] names;//names of people for comission text
    public string[] toyTypes;//types of toys that can be requested 
    public string[] colors;//Color you want for the toy
    public string[] flavorText;

    public string requestText;

    void Start()
    {
        //SetNewComission();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewComission()
    {
        int[] inputNums = new int[variableNames.Length];//random name of person, type of toy, and color, can add more perameters
        for (int i = 0; i < variableNames.Length; i++)
        {
            inputNums[i] = Random.Range(0, variableNames[i].Length);//this does the length of the word, not the array
            Debug.Log(inputNums[i]);
        }

        int sentenceStructureRand = Random.Range(0, 5);
        sentenceStructureRand = 0;//for testing
        if (sentenceStructureRand == 0)
        {
            requestText = "Hey i'm " + names[inputNums[0]] + " " + flavorText[inputNums[3]] + ". I want a " + toyTypes[inputNums[1]] + " in the color " + colors[inputNums[inputNums[2]]];
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
    }

    public void RemoveOldComission()
    {

    }
}
