using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectPickUp : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public FatigueController FatigueController;
    private Rigidbody rb;
    public Collider[] coll;
    public BoxCollider boxColl;
    public Transform player, objectContainer, fpsCam, droppedObjectsContainer;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public static bool equipped;
    public static bool slotFull;

    public GameObject interact;
    public GameObject store;

    private Vector3 originalScale;

    public GameObject currentObject;

    public LayerMask mask;

     PlayerInputs playerInputs;

    // box stuff that i need here so i can access for all the instantiated boxes
    public Transform prefabButtonParent;
    public ObjectPickUp objectPickUpScript;
    public TMP_Text Name;
    public GameObject boxUI;
    public static Transform prefabButtonParentStatic;
    public static ObjectPickUp objectPickUpScriptStatic;
    public static TMP_Text NameStatic;
    public static GameObject boxUIStatic;

    // Start is called before the first frame update
    void Start()
    {
        prefabButtonParentStatic = prefabButtonParent;
        objectPickUpScriptStatic = objectPickUpScript;
        NameStatic = Name;
        boxUIStatic = boxUI;

        mask = LayerMask.GetMask("Pickupable");
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(fpsCam.position, fpsCam.forward*pickUpRange);
        if(!equipped && lookingAtCheck.lookingAt[myInfoIndex]) // checks whether its an object to be picked up
        {
            if (!slotFull)
            {

                if (playerInputs.Player.PickUp.WasPerformedThisFrame())
                {
                    currentObject = lookingAtCheck.hitObj[myInfoIndex];
                    PickUp(currentObject);
                }
            }
        }
        else if(equipped && playerInputs.Player.PickUp.WasPerformedThisFrame()) // drop the object picked up
        {
            Drop(currentObject);
        }

        if (lookingAtCheck.lookingAt[1] && equipped) // storing object on shelf/boxes
        {
            Debug.Log("object hit: " + lookingAtCheck.hitObj[1]);
            if (lookingAtCheck.hitObj[1] != null)
            {
                if (playerInputs.Player.Interact.WasPerformedThisFrame())
                {
                    Drop(currentObject);

                    for (int i = 0; i < coll.Length - 1; i++)
                    {
                        coll[i].isTrigger = true;
                    }

                    rb.isKinematic = true;
                    currentObject.transform.position = lookingAtCheck.hitObj[1].transform.position; // 1. sets the location to the same location as the shelf slot

                    currentObject.transform.rotation = Quaternion.identity; // 2. sets all rotation axis to 0

                    float distance = ((lookingAtCheck.hitObj[1].GetComponent<BoxCollider>().size.y * lookingAtCheck.hitObj[1].transform.localScale.y) / 2) - ((boxColl.size.y * currentObject.transform.localScale.y) / 2); // calculates the distance it must travel downward to have the bottom of the object collider align with the bottom of the shelf slot collider

                    currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y - distance, currentObject.transform.position.z);
                    currentObject.transform.SetParent(lookingAtCheck.hitObj[1].transform);
                    if(currentObject.tag == "Box")
                    {
                        currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y + 0.20f, currentObject.transform.position.z);
                    }
                }
            }
        }
    }

    public void PickUp(GameObject item) // pick up function
    {
        rb = item.GetComponent<Rigidbody>();
        coll = item.GetComponents<Collider>();

        if (coll.Length == 1) // sets the box collider of the object that we use for storing
        {
            boxColl = (BoxCollider)coll[0];
        }
        else
        {
            boxColl = (BoxCollider)coll[coll.Length-1];
        }

        item.gameObject.layer = 2;

        equipped = true;
        slotFull = true;

        item.transform.SetParent(objectContainer);
        item.transform.localScale = new Vector3(item.transform.localScale.x / objectContainer.transform.localScale.x, item.transform.localScale.y / objectContainer.transform.localScale.y, item.transform.localScale.z / objectContainer.transform.localScale.z);

        item.transform.localPosition = Vector3.zero;
        FatigueController.fatigue += 3;
        
        print(FatigueController.fatigue);
        rb.isKinematic = true;
        for(int i = 0; i < coll.Length-1; i++)
        {
            coll[i].isTrigger = true;
        }
    }

    public void Drop(GameObject item) // drop item function
    {
        item.gameObject.layer = 6;

        equipped = false;
        slotFull = false;

        item.transform.SetParent(droppedObjectsContainer);
        item.transform.localScale = new Vector3(item.transform.localScale.x / droppedObjectsContainer.transform.localScale.x, item.transform.localScale.y / droppedObjectsContainer.transform.localScale.y, item.transform.localScale.z / droppedObjectsContainer.transform.localScale.z);

        rb.isKinematic = false;
        for (int i = 0; i < coll.Length - 1; i++)
        {
            coll[i].isTrigger = false;
        }

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse); // throws the object forward when dropped
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
    }
}
