using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxScript : MonoBehaviour
{
    public GameObject[] allItemGameObjects; // all pickupable items
    public Sprite[] allItemImages;

    public Transform fpsCam;
    public Transform player;
    public float Range;
    public GameObject open;
    public GameObject store;
    public MenuController menuController;
    public List<GameObject> itemsReceived = new List<GameObject>(); // items in the box

    public GameObject prefabButton; // the button for each item; same as build mode uis buttons cause they have everything i need
    public Transform prefabButtonParent;
    public Transform objectInstantiatedParent;
    public List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard

    public ObjectPickUp objectPickUpScript;
    public TMP_Text Name;

    public GameObject boxUI;

    public int index;

    private PopupInfo popupInfo;

    // Start is called before the first frame update
    void Start()
    {
        popupInfo = player.GetComponent<PopupInfo>();
        if (prefabButtonParent == null)
        {
            prefabButtonParent = ObjectPickUp.prefabButtonParentStatic;
            objectInstantiatedParent = ObjectPickUp.objectInstantiatedParentStatic;
            objectPickUpScript = ObjectPickUp.objectPickUpScriptStatic;
            Name = ObjectPickUp.NameStatic;
            boxUI = ObjectPickUp.boxUIStatic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (popupInfo.lookingAt[0])
        {
            if (popupInfo.hitObj[0] == gameObject)
            {
                //Debug.Log("worky 1");
                menuController.openPopup(open);

                if (Input.GetKeyDown(KeyCode.X)) // opens and sets up the menu for the inventory of the box
                {
                    //Debug.Log("worky 2");
                    menuController.closePopup(open);
                    menuController.openMenu(boxUI);

                    foreach(Transform child in prefabButtonParent)
                    {
                        Destroy(child.gameObject);
                    }

                    UpdateButtons();

                    if (prefabButtons.Count == 0)
                    {
                        Name.text = "Box (Empty)";
                    }
                    else
                    {
                        Name.text = "Box";
                    }
                }
                if (ObjectPickUp.equipped) // storing items in box
                {
                    Debug.Log("worky");
                    menuController.openPopup(store);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        objectPickUpScript.Drop(objectPickUpScript.currentObject);
                        itemsReceived.Add(objectPickUpScript.currentObject);
                        Destroy(objectPickUpScript.currentObject);
                    }
                }
            }
            else
            {
                menuController.closePopup(open);
                menuController.closePopup(store);
            }
        }
        else
        {
            menuController.closePopup(open);
            menuController.closePopup(store);
        }
        if (boxUI.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuController.closePopup(open);
                menuController.closePopup(store);
                menuController.closeMenu(boxUI);
            }
        }
    }

    void ObtainItem(int index, int itemObjectIndex) // whatever you want to happen when the item is pressed
    {
        GameObject itemAdded = Instantiate(allItemGameObjects[itemObjectIndex], objectInstantiatedParent);
        if(ObjectPickUp.equipped == false)
        {
            ObjectPickUp.equipped = true;
            objectPickUpScript.currentObject = itemAdded;
            objectPickUpScript.PickUp(itemAdded);
        }
        else
        {
            objectPickUpScript.Drop(objectPickUpScript.currentObject);
            objectPickUpScript.currentObject = itemAdded;
            objectPickUpScript.PickUp(itemAdded);
        }

        Destroy(prefabButtons[index]); // removes it from the hotbar
        prefabButtons.RemoveAt(index);
        itemsReceived.RemoveAt(index);

        UpdateButtons();

        menuController.closeMenu(boxUI);
    }

    public void UpdateButtons() // resets the menu to ensure that the indexes align with the corresponding index it was previously so that the item is the same item after an item before it was removed
    {
        int count = prefabButtons.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(prefabButtons[0]); // removes it from the hotbar
            prefabButtons.RemoveAt(0);
        }

        for (int i = 0; i < itemsReceived.Count; i++)
        {
            for (int j = 0; j < allItemGameObjects.Length; j++)
            {
                if (itemsReceived[i] == allItemGameObjects[j])
                {
                    int index = i;
                    int gameObjectIndex = j;

                    GameObject button = Instantiate(prefabButton, prefabButtonParent);
                    prefabButtons.Add(button);

                    Transform buttonTransform = button.transform;
                    Image itemImage = buttonTransform.GetChild(0).GetComponent<Image>();
                    Button mainButton = buttonTransform.GetChild(1).GetComponent<Button>();
                    itemImage.sprite = allItemImages[j];

                    mainButton.onClick.AddListener(() => ObtainItem(index, gameObjectIndex));
                }
            }
        }
    }
}
