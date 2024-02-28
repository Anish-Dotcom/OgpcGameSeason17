using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemProperties
{
    // which tab will it fall under:
    public bool sales = false;
    public bool combos = false;
    public bool innerParts = false;
    public bool outterParts = false;
    public bool decoratives = false;

    public bool isForWindupToy = false;
    public string itemName;
    public GameObject itemObject;
    public float itemPrice;
    public Sprite itemImage;
    public string[] daysSold; // days of the week
    public int currentItemQuantity; // quantity before buying
}

public class ItemsInShop : MonoBehaviour
{
    public float playerMoney;
    public GameObject prefabButtonInShop; // viewable button you see when you enter shop
    private List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard
    public Transform itemsToGoObject; // where the editted prefab gets instantiated

    public GameObject prefabBuying;
    public Transform prefabToGoBuyingObject;

    public Text playerMoneyText;

    public List<ItemProperties> properties = new List<ItemProperties>(); // list of all the item in shops properties

    void Start()
    {
        CreateShopButtons();
        UpdatePlayerMoney();
    }

    void CreateShopButtons()
    {
        for (int i = 0; i < properties.Count; i++) // assigns the name and price to be viewed
        {
            GameObject button = Instantiate(prefabButtonInShop, itemsToGoObject);
            prefabButtons.Add(button);

            Transform buttonTransform = button.transform;
            Image itemImage = buttonTransform.GetChild(0).GetComponent<Image>();
            Text itemNameText = buttonTransform.GetChild(1).GetComponent<Text>();
            Text itemPriceText = buttonTransform.GetChild(2).GetComponent<Text>();
            Button buyButton = buttonTransform.GetChild(3).GetComponent<Button>();

            //itemImage.sprite = properties[i].itemImage;
            itemNameText.text = properties[i].itemName;
            itemPriceText.text = properties[i].itemPrice.ToString();

            int index = i; // Capturing the correct index for whats below
            buyButton.onClick.AddListener(() => BuyItem(index));
        }
    }

    void BuyItem(int index) 
    {
        ItemProperties item = properties[index];
        if (playerMoney >= item.itemPrice)
        {
            playerMoney -= item.itemPrice;
            UpdatePlayerMoney();

            item.currentItemQuantity++;
            UpdateBuyingUI(item);
        }
        else
        {
            Debug.Log("Not enough money to buy this item!");
        }
    }

    void UpdateBuyingUI(ItemProperties item)
    {
        GameObject buyingButton = Instantiate(prefabBuying, prefabToGoBuyingObject);
        Transform buyingButtonTransform = buyingButton.transform;
        Text itemNameText = buyingButtonTransform.GetChild(0).GetComponent<Text>();
        Text itemPriceText = buyingButtonTransform.GetChild(1).GetComponent<Text>();
        Button noBuyButton = buyingButtonTransform.GetChild(3).GetComponent<Button>();

        itemNameText.text = "- " + item.itemName + " (" + item.currentItemQuantity + ")";
        itemPriceText.text = (item.itemPrice * item.currentItemQuantity).ToString();
        noBuyButton.onClick.AddListener(() => RemoveItem(item));
    }

    void RemoveItem(ItemProperties item)
    {
        item.currentItemQuantity--;
        UpdateBuyingUI(item);
        if (item.currentItemQuantity == 0)
        {
            ResetShop();
        }
        else
        {
            foreach (GameObject button in prefabButtons)
            {
                Destroy(button);
            }
            prefabButtons.Clear();
            CreateShopButtons();
        }
    }

    public void Purchase()
    {
        float totalPrice = 0;
        foreach (Transform child in prefabToGoBuyingObject)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemProperties item in properties)
        {
            totalPrice += item.itemPrice * item.currentItemQuantity;
            item.currentItemQuantity = 0;
        }

        playerMoney -= totalPrice;
        UpdatePlayerMoney();
    }

    public void UpdatePlayerMoney()
    {
        playerMoneyText.text = "$" + playerMoney.ToString();
    }

    public void ResetShop()
    {
        foreach (GameObject button in prefabButtons)
        {
            Destroy(button);
        }
        prefabButtons.Clear();
        CreateShopButtons();
    }
}

/* this is the old shop script which is so ancient I no longer understand why I did some of the things I did in it so I'm remade it.


public class ItemsInShop : MonoBehaviour
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