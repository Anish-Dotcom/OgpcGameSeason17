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

    private Dictionary<string, GameObject> instantiatedBuyingButtons = new Dictionary<string, GameObject>();
    private Dictionary<string, string> instantiatedBuyingPrices = new Dictionary<string, string>();
    private float current;
    private float decreaseamount;

    public GameObject Box;
    public Transform BoxPosition;

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
            itemPriceText.text = "$" + properties[i].itemPrice.ToString("F2");

            int index = i; // Capturing the correct index for whats below
            buyButton.onClick.AddListener(() => BuyItem(index));
        }
    }

    void BuyItem(int index) // add to the quantity of the item when bought and run the function to add it to the buying side on the right
    {
        properties[index].currentItemQuantity++;
        UpdateBuyingUI(index);
    }

    void UpdateBuyingUI(int index) // update the buying section on the right side
    {
        if (instantiatedBuyingButtons.ContainsKey(properties[index].itemName))
        {
            Destroy(instantiatedBuyingButtons[properties[index].itemName]);
            instantiatedBuyingButtons.Remove(properties[index].itemName);
            instantiatedBuyingPrices.Remove(properties[index].itemName); // where the price is stored
        }
        GameObject buyingButton = Instantiate(prefabBuying, prefabToGoBuyingObject);
        instantiatedBuyingButtons.Add(properties[index].itemName, buyingButton);
        instantiatedBuyingPrices.Add(properties[index].itemName, (properties[index].itemPrice * properties[index].currentItemQuantity).ToString("F2"));
        Transform buyingButtonTransform = buyingButton.transform;
        Text itemNameText = buyingButtonTransform.GetChild(0).GetComponent<Text>();
        Text itemPriceText = buyingButtonTransform.GetChild(1).GetComponent<Text>();
        Button noBuyButton = buyingButtonTransform.GetChild(2).GetComponent<Button>();

        itemNameText.text = "- " + properties[index].itemName + " (" + properties[index].currentItemQuantity + ")";
        itemPriceText.text = "$" + (properties[index].itemPrice * properties[index].currentItemQuantity).ToString("F2");
        noBuyButton.onClick.AddListener(() => RemoveItem(index));
    }

    void RemoveItem(int index) // pressing the button on the instantiated one in the buying section which will remove one quantity.
    {
        properties[index].currentItemQuantity--;
        UpdateBuyingUI(index);
        if (properties[index].currentItemQuantity == 0)
        {
            Destroy(instantiatedBuyingButtons[properties[index].itemName]);
            instantiatedBuyingButtons.Remove(properties[index].itemName);
            instantiatedBuyingPrices.Remove(properties[index].itemName);
        }
    }

    public void Purchase()
    {
        float totalCost = 0;
        // Iterate over the values (prices) in the instantiatedBuyingPrices dictionary
        foreach (string price in instantiatedBuyingPrices.Values)
        {
            float priceValue;
            if (float.TryParse(price, out priceValue))
            {
                totalCost += priceValue; // Add the parsed price value to the total cost
            }
        }

        current = playerMoney - totalCost;
        decreaseamount = totalCost / 300;
        StartCoroutine(textRollDown());
        GameObject box = Instantiate(Box, BoxPosition);
    }

    public void UpdatePlayerMoney() // updated the players money
    {
        playerMoneyText.text = "$" + playerMoney.ToString("F2");
    }

    public void ResetShop() // reset everything within the shop
    {
        foreach (GameObject button in prefabButtons)
        {
            Destroy(button);
        }
        prefabButtons.Clear();
        CreateShopButtons();
    }

    IEnumerator textRollDown()
    {
        yield return new WaitForSeconds(0.0001f);
        if (playerMoney <= current)
        {
            playerMoney = current;
            playerMoneyText.text = "$" + playerMoney.ToString("F2");
        }
        else
        {
            playerMoney = playerMoney - decreaseamount;
            playerMoneyText.text = "$" + playerMoney.ToString("F2");
            StartCoroutine(textRollDown());
        }
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