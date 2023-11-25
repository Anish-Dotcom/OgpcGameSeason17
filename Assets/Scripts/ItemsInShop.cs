using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInShop : MonoBehaviour
{
    public Text nameAsText;
    public Text priceAsText;
    public string itemName;
    public string price;

    public string formattedPrice;

    public GameObject prefabToInstantiate;
    public Transform parentObject;

    private List<GameObject> instantiatedObjects = new List<GameObject>();
    private int quantity = 0;

    public float totalPriceOfSingle;
    // Start is called before the first frame update
    void Start()
    {
        if (nameAsText != null && priceAsText != null)
        {
            nameAsText.text = itemName;
            priceAsText.text = "$" + price;
        }
    }

    public void buyItem()
    {
        if (prefabToInstantiate != null && parentObject != null)
        {
            quantity = quantity + 1;
            bool found = false;
            foreach (GameObject obj in instantiatedObjects)
            {
                Text[] legacyTextComponents = obj.GetComponentsInChildren<Text>();
                if (legacyTextComponents[0].text.Contains(itemName))
                {
                    legacyTextComponents[0].text = "- " + itemName + " (" + quantity + ")";
                    legacyTextComponents[1].text = "$" + formattedPrice;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                GameObject newObject = Instantiate(prefabToInstantiate, parentObject);
                instantiatedObjects.Add(newObject);
                Text[] legacyTextComponents = newObject.GetComponentsInChildren<Text>();
                legacyTextComponents[0].text = "- " + itemName + " (" + quantity + ")";
                legacyTextComponents[1].text = "$" + formattedPrice;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        totalPriceOfSingle = float.Parse(price) * (quantity + 1);
        formattedPrice = totalPriceOfSingle.ToString("F2");
    }
}
