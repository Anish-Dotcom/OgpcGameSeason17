using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObtainer : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject Box;
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
                box.transform.position = BoxPosition.transform.position;
                box.GetComponent<BoxScript>().fpsCam = fpsCam;
                box.GetComponent<BoxScript>().player = player;
                box.GetComponent<BoxScript>().open = boxInteract;
                box.GetComponent<BoxScript>().menuController = menuController;

                objectPickUp.currentObject = box;
                objectPickUp.PickUp(box);
            }
        }
    }
}
