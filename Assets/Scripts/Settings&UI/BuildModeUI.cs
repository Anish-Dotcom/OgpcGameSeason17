using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildModeItem
{
    public string itemName; // just so its easier to view in inspector
    public Sprite itemImage;
}

public class BuildModeUI : MonoBehaviour
{
    public List<BuildModeItem> items = new List<BuildModeItem>();
    public GameObject prefabButton; // the button for each item
    public Transform prefabButtonParent;
    public List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard
    public List<GameObject> itemsAdded = new List<GameObject>(); // the items that are being added to the build
    public GameObject Slots;
    public Button ScrollLeft;
    public Button ScrollRight;

    public ToyBuilder toyBuilder;

    public int index = -1;
    private int prevIndex = -2;
    private GameObject prevObj;
    private float waitTimeAfterObjChange;

    public void Update()
    {
        waitTimeAfterObjChange += Time.deltaTime;
    }
    public void addButtons(GameObject item)
    {
        itemsAdded.Add(item);
        UpdateButtons();
    }

    void AddItemToBuild(int index, GameObject itemToAdd) // whatever you want to happen when the item is pressed
    {
        //Debug.Log("pressed");
        if (waitTimeAfterObjChange > 0.25f)
        {
            waitTimeAfterObjChange = 0;
            if (prevIndex != index)
            {
                if (prevObj != null)
                {
                    if (prevObj != itemToAdd)
                    {
                        prevObj.transform.SetParent(toyBuilder.disabledStationObjsHolder.transform);
                        prevObj.SetActive(false);
                    }
                }
                prevObj = itemToAdd;
                itemToAdd.transform.SetParent(toyBuilder.heldStationObjHolder.transform);
                toyBuilder.tinkeringObj = itemToAdd;
                toyBuilder.indexer = index;
                toyBuilder.tinkering = true;
                prevIndex = index;
            }
            else
            {
                itemToAdd.transform.SetParent(toyBuilder.disabledStationObjsHolder.transform);
                toyBuilder.tinkering = false;
                prevIndex = -1;
            }
        }
    }

    public void UpdateButtons()
    {
        // Clear existing buttons
        foreach (var button in prefabButtons)
        {
            Destroy(button);
        }
        prefabButtons.Clear();

        // Rebuild buttons based on itemsAdded and items
        for (int i = 0; i < itemsAdded.Count; i++)
        {
            GameObject item = itemsAdded[i];
            for (int j = 0; j < items.Count; j++)
            {
                BuildModeItem buildModeItem = items[j];
                if (item.name.Contains(buildModeItem.itemName))
                {
                    GameObject button = Instantiate(prefabButton, prefabButtonParent);
                    prefabButtons.Add(button);

                    Transform buttonTransform = button.transform;
                    Image itemImage = buttonTransform.GetChild(0).GetComponent<Image>();
                    Button mainButton = buttonTransform.GetChild(1).GetComponent<Button>();

                    itemImage.sprite = buildModeItem.itemImage;

                    // Capture the current values of i and item in local variables
                    int capturedIndex = prefabButtons.Count - 1;
                    GameObject capturedItem = item;

                    mainButton.onClick.AddListener(() => AddItemToBuild(capturedIndex, capturedItem));
                }
            }
        }
    }

    public void ScrollLeftFunction()
    {
        HorizontalLayoutGroup layout = Slots.GetComponent<HorizontalLayoutGroup>();
        layout.padding.left += 165;
        LayoutRebuilder.ForceRebuildLayoutImmediate(Slots.GetComponent<RectTransform>());
        if (layout.padding.left == 0)
        {
            ScrollLeft.interactable = false;
        }
    }

    public void ScrollRightFunction()
    {
        ScrollLeft.interactable = true;
        HorizontalLayoutGroup layout = Slots.GetComponent<HorizontalLayoutGroup>();
        layout.padding.left -= 165;
        LayoutRebuilder.ForceRebuildLayoutImmediate(Slots.GetComponent<RectTransform>());
        if (prefabButtons.Count > 10)
        {
            ScrollRight.interactable = true;
        }
        else
        {
            ScrollRight.interactable = false;
        }
    }
}
