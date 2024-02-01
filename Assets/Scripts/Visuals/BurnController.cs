using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnController : MonoBehaviour
{
    public GameObject firePrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)//burn object
    {
        if (col.gameObject.CompareTag("Box"))
        {
            Instantiate(firePrefab, new Vector3(0, 0, 0), Quaternion.identity, col.gameObject.GetComponent<Transform>());
            //make it look like its burning and then turn to ash, add particles to the fire.
        }
    }
}
