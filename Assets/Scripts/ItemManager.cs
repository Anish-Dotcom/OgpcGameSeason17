using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Text PlayerMoneyText;
    public float PlayerMoney;
    public float payAmount;

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
        PlayerMoney = PlayerMoney - payAmount;
        PlayerMoneyText.text = "$" + PlayerMoney.ToString("F2");
        ItemsInShop.reset();
        costsStatic[0] = 0;
        costsStatic[1] = 0;
    }
}
