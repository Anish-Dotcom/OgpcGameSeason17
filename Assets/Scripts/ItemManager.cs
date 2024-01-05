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
    public static int totalAmountObj;

    public float[] costs;

    public int numItemsInShop;
    public static int[] partQuatities;//windup, spring, springgear, smallgear, screw, pole, gearboxtop, gearboxbottom, biggear
    public static int[] partAmounts;

    public GameObject[] toyParts;//windup, spring, springgear, smallgear, screw, pole, gearboxtop, gearboxbottom, biggear
    public float decreaseamount;

    void Start()
    {
        partQuatities = new int[numItemsInShop];
        partAmounts = new int[numItemsInShop];
        for (int i = 0; i < numItemsInShop; i++)
        {
            partQuatities[i] = 0;
        }
    }

    public void purchaseButton()
    {
        for (int i = 0; i < numItemsInShop; i++)
        {
            partAmounts[i] = partQuatities[i];
            payAmount += costs[i];
        }

        ItemsInShop.reset();
        for (int i = 0; i < costs.Length; i++)
        {
            costs[i] = 0;
        }
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
        for (int i = 0; i < numItemsInShop; i++)
        {
            InstantiateObjects(partAmounts[i], toyParts[i], ref posY);
            partQuatities[i] = 0;
        }

        for (int i = 0; i < totalAmountObj; i++)
        {

        }
    }

    private void InstantiateObjects(int amount, GameObject prefab, ref float posY)
    {
        if (amount > 0 && prefab != null)
        {
            for (int i = 0; i < amount; i++)
            {
                float randomX = Random.Range(-2.5f, 1.5f);//could cause objects to be in same position, better to put them in the box by making each previous one in a different calculated location based on the height of the object
                float randomZ = Random.Range(-3, -1.5f);
                Vector3 spawnPosition = new Vector3(randomX, posY, randomZ);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
                posY += 0.5f; // Increase posY by 5 for each instantiated object
            }
        }
    }
}
