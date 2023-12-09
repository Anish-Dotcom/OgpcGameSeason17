using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Text PlayerMoneyText;
    public float PlayerMoney;
    public float payAmount;
    public float current;

    public float[] costs;
    public static float[] costsStatic;
    public static int cubeQuantity;
    public static int cylinderQuantity;
    // Start is called before the first frame update
    void Start()
    {
        costsStatic = new float[costs.Length];
        cubeQuantity = 0;
        cylinderQuantity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < costsStatic.Length; i++)
        {
            costs[i] = costsStatic[i];
        }
    }

    public void purchaseButton()
    {
        payAmount = costs[0] + costs[1];
        ItemsInShop.reset();
        costsStatic[0] = 0;
        costsStatic[1] = 0;
        current = PlayerMoney - payAmount;
        StartCoroutine(textRollDown());
    }

    IEnumerator textRollDown()
    {
        yield return new WaitForSeconds(0.0001f);
        if(PlayerMoney <= current)
        {
            PlayerMoney = current;
            PlayerMoneyText.text = "$" + PlayerMoney.ToString("F2");
        }
        else
        {
            PlayerMoney = PlayerMoney - 0.01f;
            PlayerMoneyText.text = "$" + PlayerMoney.ToString("F2");
            StartCoroutine(textRollDown());
        }
    }
}
