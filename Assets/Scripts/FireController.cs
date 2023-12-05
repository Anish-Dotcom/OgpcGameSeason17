using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject[] fireObjects;
    public string objType;
    float fireHeight;
    float fireWidth;
    public float randSeed;
    public float totalTime;

    public float lightIntensity;
    public GameObject fireLight;

    void Start()
    {
        //randSeed = Random.Range(-1.0f, 1.0f);
        if (objType == "fireCon") 
        {
            fireObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                fireObjects[i] = transform.GetChild(i).gameObject;
            }
            InvokeRepeating("FireMovement", 0f, 0.08f);
        }
        else if (objType == "lightCon") 
        {
            InvokeRepeating("LightChanges", 0f, 0.08f);
        }
    }

    void Update()
    {
        totalTime += Time.deltaTime;
    }
    public void FireMovement() 
    {
        for (int i = 0; i < fireObjects.Length; i++)
        {
            //Debug.Log(totalTime);
            fireHeight = 0.7f + (Mathf.Sin((randSeed) + 2 * totalTime) / (Random.Range(6.0f, 6.2f) + i / 4));
            fireWidth = 0.7f + (Mathf.Cos((randSeed) + 2 * totalTime) / (Random.Range(8.0f, 8.2f) + i / 4));
            //Debug.Log(fireHeight + fireWidth);
            fireObjects[i].transform.localScale = new Vector3(fireWidth, fireHeight, 1);
        }
    }
    public void LightChanges() 
    { 
        fireLight.GetComponent<Light>().intensity = 3.5f + 6 * (Mathf.Sin((randSeed) + 2 * totalTime) / (Random.Range(6.0f, 6.2f)));
    }
}
