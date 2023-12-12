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

    public Sprite[] fireObjForAni1;
    public Sprite[,] fireObjectsForAnimation;//Make 2d array with all fire objects
    public int aniNum = 0;
    public bool secondRun;

    void Start()
    {
        //randSeed = Random.Range(-1.0f, 1.0f);
        if (objType == "fireCon") 
        {
            fireObjectsForAnimation = new Sprite[fireObjForAni1.Length / (fireObjForAni1.Length / 4), fireObjForAni1.Length / 4];
            for (int i = 0; i < fireObjForAni1.Length / 4; i++)//does for number of frames
            {
                for (int j = 0; j < fireObjForAni1.Length / (fireObjForAni1.Length / 4); j++)//Does for each frame
                {
                    fireObjectsForAnimation[j, i] = fireObjForAni1[j + (i * 4)];
                }
            }
            fireObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                fireObjects[i] = transform.GetChild(i).gameObject;
            }
            InvokeRepeating("FireMovement", 0f, 0.08f);
            InvokeRepeating("AnimateFire", 0f, 0.5f);
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
            fireHeight = 0.7f + (Mathf.Sin((1.3f * totalTime) + 2 * randSeed) / (Random.Range(6.0f, 6.2f) + i / 4));
            fireWidth = 0.7f + (Mathf.Cos(((1.3f * totalTime)) + 2 * randSeed) / (Random.Range(8.0f, 8.2f) + i / 4));
            //Debug.Log(fireHeight + fireWidth);
            fireObjects[i].transform.localScale = new Vector3(fireWidth, fireHeight, 1);
        }
    }
    public void LightChanges() 
    { 
        fireLight.GetComponent<Light>().intensity = 3.5f + 6 * (Mathf.Sin((1.3f * totalTime) + 2 * randSeed) / (Random.Range(6.0f, 6.2f)));
    }
    public void AnimateFire() 
    {
        secondRun = false;
        for (int i = 0; i < fireObjects.Length; i++)
        {
            fireObjects[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = fireObjectsForAnimation[i, aniNum];
            if (aniNum == 1 && !secondRun) 
            {
                //Debug.Log(fireObjects[i].transform.eulerAngles);
                Vector3 fireObj = fireObjects[i].transform.eulerAngles;
                fireObj.x *= -1f;
                fireObj.y += 180f;
                fireObj.z *= -1f;
                fireObjects[i].transform.eulerAngles = fireObj;
            }
        }
        if (aniNum == 1 && !secondRun)
        {
            secondRun = true;
        }
        if (aniNum == 2)
        {
            aniNum = 0;
        }
        else if (aniNum != 1 || secondRun)
        {
            aniNum++;
        }
        else if (aniNum == 1) 
        {
            secondRun = false;
        }
    }
}
