using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaledarScript : MonoBehaviour
{
    public int day;
    public TMP_Text daysTickMark;
    public string text;
    public ItemsInShop itemsInShop;
    public MoneyScrip MoneyScript;
    public TMP_Text OMarks;
    // Start is called before the first frame update
    void Start()
    {
        text = text + "/";
    }

    // Update is called once per frame
    void Update()
    {
        daysTickMark.text = text;
        OMarks.text = text.Replace('/', 'O') + "O";
        OMarks.text = OMarks.text.Replace(" ", "");
    }

    public void bills()
    {

        MoneyScript.DecreaseMoney(250);
    }

}
