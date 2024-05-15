using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObtainer : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject Box;
    public GameObject Tag;
    public Transform BoxPosition;

    public Transform fpsCam;
    public Transform player;
    public GameObject boxInteract;
    public MenuController menuController;

    public ObjectPickUp objectPickUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lookingAtCheck.lookingAt[myInfoIndex] && BoxPosition.childCount == 0)
        {
            if (Input.GetMouseButton(0))
            {
                GameObject box = Instantiate(Box, Vector3.zero, Quaternion.identity, BoxPosition);
                BoxScript boxScript = box.GetComponent<BoxScript>();
                box.transform.position = BoxPosition.transform.position;
                boxScript.fpsCam = fpsCam;
                boxScript.player = player;
                boxScript.open = boxInteract;
                boxScript.menuController = menuController;

                objectPickUp.currentObject = box;
                objectPickUp.PickUp(box);
                GameObject TagSlot = boxScript.TagSlot;
                GameObject tag = Instantiate(Tag, TagSlot.transform.position, Tag.transform.rotation, TagSlot.transform);
            }
        }
    }
}
