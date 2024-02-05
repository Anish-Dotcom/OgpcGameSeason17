using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnController : MonoBehaviour
{
    public GameObject[] firePrefabs;
    public GameObject fireCanvas;


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
        if (col.gameObject.CompareTag("Box"))//and hasnt been burned
        {
            int fireCanvasIndex = 0;

            int NumberOfFlames = Random.Range(1, 6);
            for (int i = 0; i < NumberOfFlames; i++)
            {
                if (i == 0)//instatiate canvas
                {
                    GameObject FireCanvas = Instantiate(fireCanvas, new Vector3(0, 0, 0), Quaternion.identity, col.gameObject.GetComponent<Transform>());
                    FireCanvas.name = "Fire Canvas";
                    for (int b = 0; b < col.gameObject.GetComponent<Transform>().childCount; b++)
                    {
                        if (col.gameObject.GetComponent<Transform>().GetChild(b).name == "Fire Canvas")
                        {
                            fireCanvasIndex = b;
                            break;
                        }
                    }
                }
                int typeOfFlame = Random.Range(0, 2);//either 0, or 1 (0 is short, 1 is tall)
                Instantiate(firePrefabs[typeOfFlame], col.gameObject.transform.position, Quaternion.identity, col.gameObject.GetComponent<Transform>().GetChild(fireCanvasIndex).GetChild(0));
            }
            //make it look like its burning and then turn to ash, add particles to the fire.
        }
    }
}