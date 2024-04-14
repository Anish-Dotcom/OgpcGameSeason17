using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildModeItem
{
    public string itemName; // just so its easier to view in inspector
    public Sprite itemImage;
    public GameObject itemObject;
}

public class BuildModeUI : MonoBehaviour
{
    public List<BuildModeItem> items = new List<BuildModeItem>();
    public GameObject prefabButton; // the button for each item
    public Transform prefabButtonParent;
    private List<GameObject> prefabButtons = new List<GameObject>(); // editted prefab of standard
    public GameObject Slots;
    public Button ScrollLeft;
    public Button ScrollRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

                int index = i; // Capturing the correct index for whats below
                mainButton.onClick.AddListener(() => AddItemToBuild(index));
            }
        }
    }

    void AddItemToBuild(int index) // whatever you want to happen when the item is pressed
    {
        Destroy(prefabButtons[index]); // removes it from the hotbar
        prefabButtons.RemoveAt(index);

        // this is where you should be instantiating the item onto the table
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
