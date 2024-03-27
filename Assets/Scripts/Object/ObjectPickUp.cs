using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
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

    private GameObject currentObject;

    public LayerMask mask;

     PlayerInputs playerInputs;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Pickupable");
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(fpsCam.position, fpsCam.forward*pickUpRange);
        if(!equipped && Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, pickUpRange, mask)) // checks whether its an object to be picked up
        {
            if (!slotFull)
            {
                interact.SetActive(true);

                if (playerInputs.Player.PickUp.WasPerformedThisFrame())
                {
                    currentObject = hit.collider.gameObject;
                    PickUp(currentObject);
                    interact.SetActive(false);
                }
            }
        }
        else if(equipped && playerInputs.Player.PickUp.WasPerformedThisFrame()) // drop the object picked up
        {
            Drop(currentObject);
        }
        else
        {
            interact.SetActive(false);
        }

        RaycastHit hit2;

        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit2, pickUpRange, LayerMask.GetMask("Storage")) && equipped) // storing object on shelf/boxes
        {
            Debug.Log("object hit: " + hit2.collider.gameObject);
            if (hit2.transform.gameObject != null)
            {
                store.SetActive(true);
                if (playerInputs.Player.Interact.WasPerformedThisFrame())
                {
                    Drop(currentObject);

                    for (int i = 0; i < coll.Length - 1; i++)
                    {
                        coll[i].isTrigger = true;
                    }

                    rb.isKinematic = true;
                    currentObject.transform.position = hit2.transform.position; // 1. sets the location to the same location as the shelf slot

                    store.SetActive(false);
                    currentObject.transform.rotation = Quaternion.identity; // 2. sets all rotation axis to 0

                    float distance = ((hit2.collider.gameObject.GetComponent<BoxCollider>().size.y * hit2.collider.gameObject.transform.localScale.y) / 2) - ((boxColl.size.y * currentObject.transform.localScale.y) / 2); // calculates the distance it must travel downward to have the bottom of the object collider align with the bottom of the shelf slot collider

                    currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y - distance, currentObject.transform.position.z);
                    currentObject.transform.SetParent(hit2.transform);
                    if(currentObject.tag == "Box")
                    {
                        currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y + 0.20f, currentObject.transform.position.z);
                    }
                }
            }
        }
        if (!Physics.Raycast(fpsCam.position, fpsCam.forward, out hit2, pickUpRange) || !equipped)
        {
            store.SetActive(false);
        }
    }

    private void PickUp(GameObject item) // pick up function
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

    private void Drop(GameObject item) // drop item function
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
