using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partLocationTrigger : MonoBehaviour
{
    public string collisionObjectName;
    public bool completed = false;
    public GameObject completedPart;
    public bool isExclusive = false;
    private List<GameObject> colliders;

    // Update is called once per frame
    private void Start()
    {
        colliders = new List<GameObject>();
    }
void Update()
    {
        completed = false;
        if (colliders != null)
        {
            foreach (GameObject thing in colliders)
            {
                if (thing != null)
                {
                    if (thing.CompareTag("Object"))
                    {
                        if (thing.name == collisionObjectName)
                                                {
                            completed = true;
                            completedPart = thing.gameObject;
                            if (!isExclusive) { break; }
                        
                        }
                        else if (isExclusive)
                        {
                            completed = false;
                            completedPart = null;
                            break;
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other.gameObject);
    }



    private void OnCollisionStay(Collision collision)
    {
    GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.CompareTag("Object") && collisionGameObject.name == collisionObjectName)
        {
            completed = true;
        }
    }


}