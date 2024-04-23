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
    public GameObject Slots;
    public Button ScrollLeft;
    public Button ScrollRight;

    public ToyBuilder toyBuilder;

    public int index = -1;
    private int prevIndex;

    public void addButtons(GameObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (item.name.Contains(items[i].itemName))
            {
                GameObject button = Instantiate(prefabButton, prefabButtonParent);
                prefabButtons.Add(button);

                Transform buttonTransform = button.transform;
                Image itemImage = buttonTransform.GetChild(0).GetComponent<Image>();
                Button mainButton = buttonTransform.GetChild(1).GetComponent<Button>();

                itemImage.sprite = items[i].itemImage;
                index = prefabButtons.Count - 1;
                mainButton.onClick.AddListener(() => AddItemToBuild(index, item));
            }
        }
    }

    void AddItemToBuild(int index, GameObject itemToAdd) // whatever you want to happen when the item is pressed
    {
        //Debug.Log("pressed");
        if (prevIndex != index)
        {
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
