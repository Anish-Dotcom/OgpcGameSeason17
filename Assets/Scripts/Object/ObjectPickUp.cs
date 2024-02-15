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

    public bool equipped;
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
        mask = LayerMask.GetMask("Pickupable");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(fpsCam.position, fpsCam.forward*pickUpRange);
        if(!equipped && Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, pickUpRange, mask))
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
        else if(equipped && Input.GetMouseButtonDown(0))
        {
            Drop(currentObject);
        }
        else
        {
            interact.SetActive(false);
        }

        RaycastHit hit2;

        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit2, pickUpRange) && equipped)
        {
            foreach (GameObject locationOnShelf in LocationsOnShelf)
            {
                if (hit2.collider.gameObject == locationOnShelf)
                {
                    store.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E)) // storing on shelf
                    {
                        Drop(currentObject);
                        rb.isKinematic = true;
                        currentObject.transform.position = locationOnShelf.transform.position;
                        Debug.Log(locationOnShelf.transform.position);
                        store.SetActive(false);
                        currentObject.transform.rotation = Quaternion.identity;

                        float distance = (currentObject.transform.position.y - boxColl.size.y / 2) - locationOnShelf.transform.position.y;
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

    private void PickUp(GameObject item)
    {
        rb = item.GetComponent<Rigidbody>();
        coll = item.GetComponents<Collider>();

        if (coll.Length == 1)
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
        FatigueController.fatigue += 20;
        print(FatigueController.fatigue);
        rb.isKinematic = true;
        coll[0].isTrigger = true;
    }

    private void Drop(GameObject item)
    {
        item.gameObject.layer = 6;

        equipped = false;
        slotFull = false;

        item.transform.SetParent(null);

        rb.isKinematic = false;
        coll[0].isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        item.transform.localScale = originalScale;
    }
}
