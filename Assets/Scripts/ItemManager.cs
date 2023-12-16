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
    public static int winduppartQuantity;
    public static int springQuantity;
    public static int springgearQuantity;
    public static int smallgearQuantity;
    public static int screwQuantity;
    public static int poleQuantity;
    public static int gearboxtopQuantity;
    public static int gearboxbottomQuantity;
    public static int biggearQuantity;

    public int winduppartAmount; // i only have these duplicates because i wanted to see if it was working by checking the inspector
    public int springAmount;
    public int springgearAmount;
    public int smallgearAmount;
    public int screwAmount;
    public int poleAmount;
    public int gearboxtopAmount;
    public int gearboxbottomAmount;
    public int biggearAmount;

    public float decreaseAmount;
    // Start is called before the first frame update
    void Start()
    {
        costsStatic = new float[costs.Length];
        winduppartQuantity = 0;
        springQuantity = 0;
        springgearQuantity = 0;
        smallgearQuantity = 0;
        screwQuantity = 0;
        poleQuantity = 0;
        gearboxtopQuantity = 0;
        gearboxbottomQuantity = 0;
        biggearQuantity = 0;
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
        winduppartAmount = winduppartQuantity;
        springAmount = springQuantity;
        springgearAmount = springgearQuantity;
        smallgearAmount = smallgearQuantity;
        screwAmount = screwQuantity;
        poleAmount = poleQuantity;
        gearboxtopAmount = gearboxtopQuantity;
        gearboxbottomAmount = gearboxbottomQuantity;
        biggearAmount = biggearQuantity;

        payAmount = costs[0] + costs[1] + costs[2] + costs[3] + costs[4] + costs[5] + costs[6] + costs[7] + costs[8];
        ItemsInShop.reset();
        costsStatic[0] = 0;
        costsStatic[1] = 0;
        costsStatic[2] = 0;
        costsStatic[3] = 0;
        costsStatic[4] = 0;
        costsStatic[5] = 0;
        costsStatic[6] = 0;
        costsStatic[7] = 0;
        costsStatic[8] = 0;
        current = PlayerMoney - payAmount;
        decreaseAmount = payAmount / 300;
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
            PlayerMoney = PlayerMoney - decreaseAmount;
            PlayerMoneyText.text = "$" + PlayerMoney.ToString("F2");
            StartCoroutine(textRollDown());
        }
    }
}
