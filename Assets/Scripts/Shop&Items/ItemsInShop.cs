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
    private bool sales = true;
    private bool combos = true;
    private bool innerParts = true;
    private bool outerParts = true;
    private bool decoratives = true;

    public GameObject prefabButtonInShop; // viewable button you see when you enter shop
    private List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard
    public Transform itemsToGoObject; // where the editted prefab gets instantiated
    public GameObject itemsToGoGameObject;
    public GameObject BuyingGameObject;

    public GameObject prefabBuying;
    public Transform prefabToGoBuyingObject;

    public List<ItemProperties> properties = new List<ItemProperties>(); // list of all the item in shops properties

    private Dictionary<string, GameObject> instantiatedBuyingButtons = new Dictionary<string, GameObject>();
    private Dictionary<string, string> instantiatedBuyingPrices = new Dictionary<string, string>();

    public GameObject Box;
    public Transform BoxPosition;
    public Transform fpsCam;
    public Transform player;
    public GameObject boxInteract;
    public MenuController menuController;

    public MoneyScrip MoneyScript;

    public Button scrollUp;
    public Button scrollDown;
    public Button scrollUpBuying;
    public Button scrollDownBuying;

    void Start()
    {
        CreateShopButtonsAllTab(); // starts it with the all tab
    }

    void CreateShopButtonsTabs() // for the individual tabs
    {
        for (int i = 0; i < properties.Count; i++) // assigns the name and price to be viewed
        {
            if (sales)
            {
                if (properties[i].sales)
                {
                    CreateShopButtons(i);
                }
            }
            else if (combos)
            {
                if (properties[i].combos)
                {
                    CreateShopButtons(i);
                }
            }
            else if (innerParts)
            {
                if (properties[i].innerParts)
                {
                    CreateShopButtons(i);
                }
            }
            else if (outerParts)
            {
                if (properties[i].outterParts)
                {
                    CreateShopButtons(i);
                }
            }
            else if (decoratives)
            {
                if (properties[i].decoratives)
                {
                    CreateShopButtons(i);
                }
            }
        }
        GridLayoutGroup layout = itemsToGoGameObject.GetComponent<GridLayoutGroup>(); // makes sure it starts the shop grid from the top
        layout.padding.top = 81;
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemsToGoGameObject.GetComponent<RectTransform>());
    }

    void CreateShopButtonsAllTab() // specifically for the all tab. i needed this to be a seperate function to prevent the same items in 2 different tabs from appearing twice
    {
        for (int i = 0; i < properties.Count; i++) // assigns the name and price to be viewed
        {
            CreateShopButtons(i);
        }
        GridLayoutGroup layout = itemsToGoGameObject.GetComponent<GridLayoutGroup>(); // makes sure it starts the shop grid from the top
        layout.padding.top = 81;
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemsToGoGameObject.GetComponent<RectTransform>());
    }

    void CreateShopButtons(int i)
    {
        GameObject button = Instantiate(prefabButtonInShop, itemsToGoObject);
        prefabButtons.Add(button);

        Transform buttonTransform = button.transform;
        Image itemImage = buttonTransform.GetChild(1).GetComponent<Image>();
        Text itemNameText = buttonTransform.GetChild(2).GetComponent<Text>();
        Text itemPriceText = buttonTransform.GetChild(3).GetComponent<Text>();
        Button buyButton = buttonTransform.GetChild(4).GetComponent<Button>();
        Image iswindup = buttonTransform.GetChild(5).GetComponent<Image>();

        if (properties[i].isForWindupToy)
        {
            iswindup.gameObject.SetActive(true);
        }
        else
        {
            iswindup.gameObject.SetActive(false);
        }

        itemImage.sprite = properties[i].itemImage;
        itemNameText.text = properties[i].itemName;
        itemPriceText.text = "$" + properties[i].itemPrice.ToString("F2");

        int index = i; // Capturing the correct index for whats below
        buyButton.onClick.AddListener(() => BuyItem(index));
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
        if (properties[index].currentItemQuantity <= 0)
        {
            Destroy(instantiatedBuyingButtons[properties[index].itemName]);
            instantiatedBuyingButtons.Remove(properties[index].itemName);
            instantiatedBuyingPrices.Remove(properties[index].itemName);
            properties[index].currentItemQuantity = 0;
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

        GameObject box = Instantiate(Box, Vector3.zero, Quaternion.identity, BoxPosition);
        box.transform.position = BoxPosition.transform.position;
        box.GetComponent<BoxScript>().fpsCam = fpsCam;
        box.GetComponent<BoxScript>().player = player;
        box.GetComponent<BoxScript>().open = boxInteract;
        box.GetComponent<BoxScript>().menuController = menuController;

        foreach (ItemProperties item in properties)
        {
            if(item.currentItemQuantity > 0)
            {
                for(int i = 0; i < item.currentItemQuantity; i++)
                {
                    BoxScript boxScript = box.GetComponent<BoxScript>();
                    boxScript.itemsReceived.Add(item.itemObject);
                }
                item.currentItemQuantity = 0;
            }
        }

        for(int i = 0; i < properties.Count; i++)
        {
            RemoveItem(i);
        }

        MoneyScript.DecreaseMoney(totalCost);
    }

    public void ChangeColliderTrigger(GameObject collObj, bool setToType)
    {
        Collider[] coll = collObj.GetComponents<Collider>();

        if (coll.Length == 1)//only a box collider
        {
            coll[0].isTrigger = setToType;
        }
        else//also another type
        {
            if (coll[0].GetType() == typeof(BoxCollider))//if is a box collider, make other not a trigger
            {
                coll[1].isTrigger = setToType;
            }
            else//2nd collider is a box collider, 1st is other type, !!!- means that there cannot be two box colliders
            {
                coll[0].isTrigger = setToType;
            }
        }
    }

    public void allTab() // tabs
    {
        sales = true;
        combos = true;
        innerParts = true;
        outerParts = true;
        decoratives = true;
        resetShopButtons();
        CreateShopButtonsAllTab();
    }
    public void salesTab()
    {
        sales = true;
        combos = false;
        innerParts = false;
        outerParts = false;
        decoratives = false;
        resetShopButtons();
        CreateShopButtonsTabs();
    }
    public void combosTab()
    {
        sales = false;
        combos = true;
        innerParts = false;
        outerParts = false;
        decoratives = false;
        resetShopButtons();
        CreateShopButtonsTabs();
    }
    public void innerPartsTab()
    {
        sales = false;
        combos = false;
        innerParts = true;
        outerParts = false;
        decoratives = false;
        resetShopButtons();
        CreateShopButtonsTabs();
    }
    public void outerPartsTab()
    {
        sales = false;
        combos = false;
        innerParts = false;
        outerParts = true;
        decoratives = false;
        resetShopButtons();
        CreateShopButtonsTabs();
    }
    public void decorativesTab()
    {
        sales = false;
        combos = false;
        innerParts = false;
        outerParts = false;
        decoratives = true;
        resetShopButtons();
        CreateShopButtonsTabs();
    }

    public void resetShopButtons() // resets the buttons in the shop (useful for the tabs)
    {
        foreach(GameObject button in prefabButtons)
        {
            Destroy(button);
        }
        prefabButtons.Clear();
    }

    public void ScrollUpFunction()
    {
        if (itemsToGoGameObject != null)
        {
            GridLayoutGroup layout = itemsToGoGameObject.GetComponent<GridLayoutGroup>();
            layout.padding.top += 159;
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsToGoGameObject.GetComponent<RectTransform>());
            if (layout.padding.top == 81)
            {
                scrollUp.interactable = false;
            }
        }
    }

    public void ScrollDownFunction()
    {
        if (itemsToGoGameObject != null)
        {
            scrollUp.interactable = true;
            GridLayoutGroup layout = itemsToGoGameObject.GetComponent<GridLayoutGroup>();
            layout.padding.top -= 159;
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsToGoGameObject.GetComponent<RectTransform>());
        }
    }

    public void ScrollUpBUYINGFunction()
    {
        VerticalLayoutGroup layout = BuyingGameObject.GetComponent<VerticalLayoutGroup>();
        layout.padding.top += 40;
        LayoutRebuilder.ForceRebuildLayoutImmediate(BuyingGameObject.GetComponent<RectTransform>());
        if (layout.padding.top == 0)
        {
            scrollUpBuying.interactable = false;
        }
    }

    public void ScrollDownBUYINGFunction()
    {
        scrollUpBuying.interactable = true;
        VerticalLayoutGroup layout = BuyingGameObject.GetComponent<VerticalLayoutGroup>();
        layout.padding.top -= 40;
        LayoutRebuilder.ForceRebuildLayoutImmediate(BuyingGameObject.GetComponent<RectTransform>());
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