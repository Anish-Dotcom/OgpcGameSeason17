using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public FatigueController FatigueController;

    public Rigidbody rb;
    public Collider coll;
    public Transform player, objectContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    public GameObject interact;
    public GameObject store;

    public Vector3 originalScale;

    public GameObject[] LocationsOnShelf;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!equipped)
        {
            transform.localScale = originalScale;
        }

        RaycastHit hit;
        Ray ray = new Ray(fpsCam.position, fpsCam.forward);

        Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped && Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.gameObject == gameObject && !slotFull)
            {
                interact.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    PickUp();
                    interact.SetActive(false);
                }
            }
        }
        else if(equipped && Input.GetMouseButtonDown(0))
        {
            transform.localScale = originalScale;
            Drop();
        }
        else
        {
            interact.SetActive(false);
        }

        RaycastHit hit2;
        Ray ray2 = new Ray(fpsCam.position, fpsCam.forward);

        if (Physics.Raycast(ray, out hit2, pickUpRange) && equipped)
        {
            foreach (GameObject locationOnShelf in LocationsOnShelf)
            {
                if (hit2.collider.gameObject == locationOnShelf)
                {
                    store.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Drop();
                        rb.isKinematic = true;
                        transform.position = locationOnShelf.transform.position;
                        store.SetActive(false);
                    }
                }
            }
        }
        else
        {
            store.SetActive(false);
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(objectContainer);
        transform.localPosition = Vector3.zero;
        FatigueController.fatigue += 20;
        print(FatigueController.fatigue);
        rb.isKinematic = true;
        coll.isTrigger = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
    }
}
