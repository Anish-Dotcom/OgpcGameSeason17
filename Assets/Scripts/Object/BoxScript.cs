using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxScript : MonoBehaviour
{
    public Transform fpsCam;
    public Transform player;
    public float Range;
    public GameObject interact;
    public MenuController menuController;
    public List<ItemProperties> itemsContained = new List<ItemProperties>(); // items in the box

    public GameObject prefabButton; // the button for each item; same as build mode uis buttons cause they have everything i need
    public Transform prefabButtonParent;
    private List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard

    public ObjectPickUp objectPickUpScript;
    public TMP_Text Name;

    public GameObject boxUI;

    // Start is called before the first frame update
    void Start()
    {
        if (prefabButtonParent == null)
        {
            prefabButtonParent = ObjectPickUp.prefabButtonParentStatic;
            objectPickUpScript = ObjectPickUp.objectPickUpScriptStatic;
            Name = ObjectPickUp.NameStatic;
            boxUI = ObjectPickUp.boxUIStatic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(fpsCam.position, fpsCam.forward);

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (Physics.Raycast(ray, out hit, Range) && !ObjectPickUp.slotFull)
        {
            if (hit.collider.gameObject == gameObject)
            {
                menuController.openPopup(interact);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    menuController.closePopup(interact);
                    menuController.openMenu(boxUI);

                    for(int i = 0; i < prefabButtons.Count; i++)
                    {
                        Destroy(prefabButtons[i]); // removes it from the hotbar
                        prefabButtons.RemoveAt(i);
                    }

                    for (int i = 0; i < itemsContained.Count; i++) {
                        GameObject button = Instantiate(prefabButton, prefabButtonParent);
                        prefabButtons.Add(button);

                        Transform buttonTransform = button.transform;
                        Image itemImage = buttonTransform.GetChild(0).GetComponent<Image>();
                        Button mainButton = buttonTransform.GetChild(1).GetComponent<Button>();

                        itemImage.sprite = itemsContained[i].itemImage;

                        int index = i; // Capturing the correct index for whats below
                        mainButton.onClick.AddListener(() => ObtainItem(index));
                    }

                    if(itemsContained.Count == 0)
                    {
                        Name.text = "Box (Empty)";
                    }
                    else
                    {
                        Name.text = "Box";
                    }
                }
            }
            else
            {
                menuController.closePopup(interact);
            }
        }
        else
        {
            menuController.closePopup(interact);
        }
        if (boxUI.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuController.closeMenu(boxUI);
            }
        }
    }

    void ObtainItem(int index) // whatever you want to happen when the item is pressed
    {
        GameObject itemAdded = Instantiate(itemsContained[index].itemObject, prefabButtonParent);
        if(ObjectPickUp.slotFull == false)
        {
            objectPickUpScript.PickUp(itemAdded);
        }
        else
        {
            objectPickUpScript.Drop(objectPickUpScript.currentObject);
            objectPickUpScript.PickUp(itemAdded);
        }

        Destroy(prefabButtons[index]); // removes it from the hotbar
        prefabButtons.RemoveAt(index);
        itemsContained.RemoveAt(index);
    }
}
