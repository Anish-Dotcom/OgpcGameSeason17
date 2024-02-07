using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public FatigueController FatigueController;

    private Rigidbody rb;
    private BoxCollider coll;
    public Transform player, objectContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    public GameObject interact;
    public GameObject store;

    public Vector3 originalScale;

    public GameObject[] LocationsOnShelf;

    private GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(fpsCam.position, fpsCam.forward);

        if(!equipped && Physics.Raycast(ray, out hit, pickUpRange))
        {
            Debug.Log("asdf");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pickupable"))
            {

                if (hit.collider.gameObject == gameObject && !slotFull)
                {
                    interact.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        currentObject = hit.collider.gameObject;
                        PickUp(currentObject);
                        interact.SetActive(false);
                    }
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
        Ray ray2 = new Ray(fpsCam.position, fpsCam.forward);

        if (Physics.Raycast(ray2, out hit2, pickUpRange) && equipped)
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
                        store.SetActive(false);
                        currentObject.transform.rotation = Quaternion.identity;

                        float distance = (transform.position.y-coll.size.y/2)-locationOnShelf.transform.position.y;

                        currentObject.transform.position = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
                    }
                }
            }
        }
        else if (!Physics.Raycast(ray2, out hit2, pickUpRange) && !equipped)
        {
            store.SetActive(false);
        }
    }

    private void PickUp(GameObject item)
    {
        rb = item.GetComponent<Rigidbody>();
        coll = item.GetComponent<BoxCollider>();

        item.gameObject.layer = 2;

        equipped = true;
        slotFull = true;

        item.transform.SetParent(objectContainer);
        item.transform.localPosition = Vector3.zero;
        FatigueController.fatigue += 20;
        print(FatigueController.fatigue);
        rb.isKinematic = true;
        coll.isTrigger = true;
    }

    private void Drop(GameObject item)
    {
        item.gameObject.layer = 6;

        equipped = false;
        slotFull = false;

        item.transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
    }
}
