using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject[] fireObjects;
    public string objType;
    int fireHeight;
    int fireWidth;
    public int[,,] fireSize;
    public float randSeed;

    void Start()
    {
        randSeed = Random.Range(-1.0f, 1.0f);
        fireObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            fireObjects[i] = transform.GetChild(i).gameObject;
        }

        fireSize = new int[fireObjects.Length, 10, 2];
        for (int b = 0; b < fireSize.GetLength(0); b++)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                //fireSize[i, b, 1] = Mathf.Sin(i * randSeed);
                //fireSize[i, b, 2] = Mathf.Sin(i * randSeed);
            }
        }
    }

    void Update()
    {
        if (objType == "fireCon") 
        {
            StartCoroutine(FireWait());
            for (int b = 0; b < fireSize.GetLength(0); b++) 
            {
                StartCoroutine(FireWait());
                for (int i = 0; i < fireObjects.Length; i++)
                {
                    fireHeight = fireSize[i, b, 1];
                    fireWidth = fireSize[i, b, 2];
                    fireObjects[i].transform.localScale = new Vector3(fireWidth, fireHeight, 1);
                }
            }
        }
    }
    IEnumerator FireWait()
    {
        yield return new WaitForSeconds(0.25f);
    }
}
