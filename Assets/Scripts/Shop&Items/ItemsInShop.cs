using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemProperties
{
    public bool isForWindupToy;
    public string itemName;
    public GameObject itemObject;
    public float itemPrice;
    public Sprite itemImage;
    public string[] daysSold; // days of the week
}

public class ItemsInShop : MonoBehaviour
{
    public List<ItemProperties> properties = new List<ItemProperties>();

    void Start()
    {
        
    }
}

/*public class ItemsInShop : MonoBehaviour
{
    public Text nameAsText;
    public Text priceAsText;
    public string itemName;
    public string price;

    public string formattedPrice;

    public GameObject prefabToInstantiate;
    public Transform parentObject;

    private static List<GameObject> instantiatedObjects = new List<GameObject>();
    private int quantity = 0;

    public float totalPriceOfSingle;

    public GameObject Telephone;

    // Start is called before the first frame update
    void Start()
    {
        if (nameAsText != null && priceAsText != null)
        {
            nameAsText.text = itemName;
            priceAsText.text = "$" + price;
        }
        Button[] buttons = prefabToInstantiate.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button.gameObject));
        }
    }

    public void buyItem()
    {
        if (prefabToInstantiate != null && parentObject != null)
        {
            quantity++;
            ItemManager.totalAmountObj++;
            bool found = false;
            foreach (GameObject obj in instantiatedObjects)
            {
                Text[] legacyTextComponents = obj.GetComponentsInChildren<Text>();
                if (legacyTextComponents[0].text.Contains(itemName))
                {
                    legacyTextComponents[0].text = "- " + itemName + " (" + quantity + ")";
                    reformatThePrice();
                    legacyTextComponents[1].text = "$" + formattedPrice;
                    found = true;
                    UpdateArray();
                    break;
                }
            }
            if (!found)
            {
                GameObject newObject = Instantiate(prefabToInstantiate, parentObject);
                instantiatedObjects.Add(newObject);
                Text[] legacyTextComponents = newObject.GetComponentsInChildren<Text>();
                legacyTextComponents[0].text = "- " + itemName + " (" + quantity + ")";
                reformatThePrice();
                legacyTextComponents[1].text = "$" + formattedPrice;
                UpdateArray();

                Button newButton = newObject.GetComponentInChildren<Button>();
                if (newButton != null)
                {
                    newButton.onClick.AddListener(() => OnButtonClick(newButton.gameObject));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reformatThePrice()
    {
        totalPriceOfSingle = float.Parse(price) * (quantity);
        formattedPrice = totalPriceOfSingle.ToString("F2");
    }

    void UpdateUI(GameObject obj)
    {
        Text[] legacyTextComponents = obj.GetComponentsInChildren<Text>();
        legacyTextComponents[0].text = "- " + itemName + " (" + quantity + ")";
        legacyTextComponents[1].text = "$" + formattedPrice;
    }

    public void UpdateArray()
    {
        for (int i = 0; i < Telephone.GetComponent<ItemManager>().numItemsInShop; i++)
        {
            ItemManager.partQuatities[i] = quantity;
            Telephone.GetComponent<ItemManager>().costs[i] = float.Parse(formattedPrice);
        }
    }

    void OnButtonClick(GameObject buttonClicked)
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            if (obj == buttonClicked.transform.parent.gameObject)
            {
                quantity--;
                ItemManager.totalAmountObj--;
                reformatThePrice();
                UpdateUI(obj);
                UpdateArray();

                if (quantity <= 0)
                {
                    instantiatedObjects.Remove(obj);
                    Destroy(obj);
                }

                break;
            }
        }
    }

    public static void reset()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
        foreach (ItemsInShop item in FindObjectsOfType<ItemsInShop>())
        {
            item.quantity = 0;
        }
    }
}*/