using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MoneyScrip : MonoBehaviour
{
    public float currentMoney;
    public float unChangedMoney;
    public Text moneyText;
    public float amount2;
    // Start is called before the first frame update
    void Start()
    {

        DecreaseMoney(10);
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + currentMoney.ToString("F2");
    }




    public void IncreaseMoney(float amount)
    {
        unChangedMoney = currentMoney;
        amount2 = amount;

        InvokeRepeating("TextRollUp", 0, 0.0001f);


    }
    public void TextRollUp()
    {
        currentMoney += amount2 / 200;
        if (currentMoney > unChangedMoney + amount2)
        {
            currentMoney = unChangedMoney + amount2;
            CancelInvoke("TextRollUp");
        }
    }
    public void DecreaseMoney(float amount)
    {
        unChangedMoney = currentMoney;
        amount2 = amount;

        InvokeRepeating("TextRollDown", 0, 0.0001f);


    }
    public void TextRollDown()
    {
        currentMoney -= amount2 / 200;
        if (currentMoney < unChangedMoney - amount2)
        {
            currentMoney = unChangedMoney - amount2;
            CancelInvoke("TextRollDown");
        }
    }

}
