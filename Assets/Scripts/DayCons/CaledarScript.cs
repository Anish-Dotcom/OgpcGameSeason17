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
    // Start is called before the first frame update
    void Start()
    {
        text = text + "/";
    }

    // Update is called once per frame
    void Update()
    {
        daysTickMark.text = text;
    }

    public void bills()
    {

        MoneyScript.DecreaseMoney(250);
    }
}
