using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComissionController : MonoBehaviour
{
    public List<int> inputNums = new List<int>();

    public string[] names;//names of people for comission text
    public string[] toyTypes;//types of toys that can be requested 
    public string[] colors;//Color you want for the toy
    public Vector3[] colorRGBValues;
    public string[] flavorText;

    public string requestText;
    public int numberOfComissions;
    public TMP_Text toDoPoster;


    void Start()
    {
        numberOfComissions = 0;
        SetNewComission();
        SetNewComission();
    }

    public void SetNewComission()
    {
        numberOfComissions++;

        inputNums.Add(Random.Range(0, names.Length));
        inputNums.Add(Random.Range(0, toyTypes.Length));
        inputNums.Add(Random.Range(0, colors.Length));
        inputNums.Add(Random.Range(0, flavorText.Length));

        int sentenceStructureRand = Random.Range(0, 5);//can be int 0, 1, 2, 3, or 4
        sentenceStructureRand = 0;//for testing
        if (sentenceStructureRand == 0)
        {
            requestText = "Hey I'm " + names[inputNums[Current(0)]] + ". " + flavorText[inputNums[Current(3)]] + " I want a " + toyTypes[inputNums[Current(1)]] + " in the color " + colors[inputNums[Current(2)]] + ".";
            Debug.Log(requestText);
        }
        else if (sentenceStructureRand == 1)//do other sentence structures later
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
        string simplifiedText = names[inputNums[Current(0)]] + " requested a " + colors[inputNums[Current(2)]] + ", " + toyTypes[inputNums[Current(1)]] + ".";
        if (numberOfComissions == 1)//this is the only commission
        {
            toDoPoster.text = "- " + simplifiedText;
        }
        else
        {
            toDoPoster.text = toDoPoster.text + "\n\n" + "- " + simplifiedText;
        }
    }

    public void SetChildComission()//set a comm
    {

    }

    public void RemoveOldComission(int commissionIndex)
    {
        numberOfComissions--;
        for (int i = commissionIndex; i < commissionIndex + 4; i++)
        {
            inputNums.RemoveAt(i);//removes the inputs assosiated with finished comission
        }
    }

    public int Current(int input)
    {
        int current = input + (4 * (numberOfComissions - 1));
        return current;
    }
}
