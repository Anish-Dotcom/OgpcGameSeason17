using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public FatigueController FatigueController;
    private Rigidbody rb;
    public Collider[] coll;
    public BoxCollider boxColl;
    public Transform player, objectContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public static bool equipped;
    public static bool slotFull;

    public GameObject interact;
    public GameObject store;

    private Vector3 originalScale;

    public GameObject[] LocationsOnShelf;
    public GameObject[] pickupableObject;
    private GameObject currentObject;

    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        //CollectObjectsWithTagOnStart();
        mask = LayerMask.GetMask("Pickupable");
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

                if (Input.GetMouseButtonDown(0))
                {
                    originalScale = hit.transform.localScale;
                    currentObject = hit.collider.gameObject;
                    PickUp(currentObject);
                    interact.SetActive(false);
                }
            }
        }
        else if(equipped && Input.GetMouseButtonDown(0)) // drop the object picked up
        {
            Drop(currentObject);
        }
        else
        {
            interact.SetActive(false);
        }

        RaycastHit hit2;

        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit2, pickUpRange) && equipped) // storing object on shelf/boxes
        {
            foreach (GameObject locationOnShelf in LocationsOnShelf)
            {
                if (hit2.collider.gameObject == locationOnShelf)
                {
                    store.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Drop(currentObject);
                        rb.isKinematic = true;
                        currentObject.transform.position = locationOnShelf.transform.position; // 1. sets the location to the same location as the shelf slot
                        
                        store.SetActive(false);
                        currentObject.transform.rotation = Quaternion.identity; // 2. sets all rotation axis to 0

                        float distance = ((locationOnShelf.GetComponent<BoxCollider>().size.y * locationOnShelf.transform.localScale.y)/2) - ((boxColl.size.y * currentObject.transform.localScale.y)/2); // calculates the distance it must travel downward to have the bottom of the object collider align with the bottom of the shelf slot collider
                        
                        currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y - distance, currentObject.transform.position.z);
                    }
                }
            }
        }
        if (!Physics.Raycast(fpsCam.position, fpsCam.forward, out hit2, pickUpRange) || !equipped)
        {
            store.SetActive(false);
        }
    }

    /*public void CollectObjectsWithTagOnStart() // putting the boxes store slots into the shelf slots array (i used 2 functions)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Box");

        int currentLength = LocationsOnShelf != null ? LocationsOnShelf.Length : 0;
        int additionalLocations = 0;

        if (objectsWithTag.Length > 0)
        {
            foreach (GameObject obj in objectsWithTag)
            {
                additionalLocations += Mathf.Min(3, obj.transform.childCount);
            }

            int newSize = currentLength + additionalLocations;

            System.Array.Resize(ref LocationsOnShelf, newSize);

            int index = currentLength;

            foreach (GameObject obj in objectsWithTag)
            {
                for (int i = 0; i < Mathf.Min(3, obj.transform.childCount); i++)
                {
                    LocationsOnShelf[index] = obj.transform.GetChild(i).gameObject;
                    index++;
                }
            }
        }
        else
        {
            Debug.LogWarning("No objects found with tag: Box");
        }
    }*/

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
            boxColl = (BoxCollider)coll[1];
        }

        item.gameObject.layer = 2;

        equipped = true;
        slotFull = true;

        item.transform.SetParent(objectContainer);
        item.transform.localPosition = Vector3.zero;
        FatigueController.fatigue += 3;
        
        print(FatigueController.fatigue);
        rb.isKinematic = true;
        coll[0].isTrigger = true;
    }

    private void Drop(GameObject item) // drop item function
    {
        item.gameObject.layer = 6;

        equipped = false;
        slotFull = false;

        item.transform.SetParent(null);

        rb.isKinematic = false;
        coll[0].isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse); // throws the object forward when dropped
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        item.transform.localScale = originalScale;
    }
}
