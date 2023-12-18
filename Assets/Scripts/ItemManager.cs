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

    public int winduppartAmount;
    public int springAmount;
    public int springgearAmount;
    public int smallgearAmount;
    public int screwAmount;
    public int poleAmount;
    public int gearboxtopAmount;
    public int gearboxbottomAmount;
    public int biggearAmount;

    public GameObject winduppart;
    public GameObject spring;
    public GameObject springgear;
    public GameObject smallgear;
    public GameObject screw;
    public GameObject pole;
    public GameObject gearboxtop;
    public GameObject gearboxbottom;
    public GameObject biggear;

    public float decreaseamount;
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
        decreaseamount = payAmount / 300;
        getSent();
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
            PlayerMoney = PlayerMoney - decreaseamount;
            PlayerMoneyText.text = "$" + PlayerMoney.ToString("F2");
            StartCoroutine(textRollDown());
        }
    }

    public void getSent()
    {
        float posY = 0.5f;

        // Instantiate objects based on their amounts
        InstantiateObjects(winduppartAmount, winduppart, ref posY);
        InstantiateObjects(springAmount, spring, ref posY);
        InstantiateObjects(springgearAmount, springgear, ref posY);
        InstantiateObjects(smallgearAmount, smallgear, ref posY);
        InstantiateObjects(screwAmount, screw, ref posY);
        InstantiateObjects(poleAmount, pole, ref posY);
        InstantiateObjects(gearboxtopAmount, gearboxtop, ref posY);
        InstantiateObjects(gearboxbottomAmount, gearboxbottom, ref posY);
        InstantiateObjects(biggearAmount, biggear, ref posY);
    }

    private void InstantiateObjects(int amount, GameObject prefab, ref float posY)
    {
        if (amount > 0 && prefab != null)
        {
            for (int i = 0; i < amount; i++)
            {
                float randomX = Random.Range(-2.5f, 1.5f);
                float randomZ = Random.Range(-3, -1.5f);
                Vector3 spawnPosition = new Vector3(randomX, posY, randomZ);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
                posY += 0.5f; // Increase posY by 5 for each instantiated object
            }
        }
    }
}
