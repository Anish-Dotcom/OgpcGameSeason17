using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public Transform fpsCam;
    public Transform player;
    public float Range;
    public GameObject interact;
    public GameObject boxTop1;
    public GameObject boxTop2;
    public GameObject boxTop1onopen;
    public GameObject boxTop2onopen;
    public GameObject tape;
    public bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(fpsCam.position, fpsCam.forward);

        Vector3 distanceToPlayer = player.position - transform.position;
        if (Physics.Raycast(ray, out hit, Range) && !isOpened && !ObjectPickUp.slotFull)
        {
            if (hit.collider.gameObject == gameObject)
            {
                interact.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isOpened = true;
                    interact.SetActive(false);

                    tape.SetActive(false);
                    boxTop1.SetActive(false);
                    boxTop2.SetActive(false);

                    boxTop1onopen.SetActive(true);
                    boxTop2onopen.SetActive(true);
                }
            }
            else
            {
                interact.SetActive(false);
            }
        }
        else
        {
            interact.SetActive(false);
        }
    }
}
