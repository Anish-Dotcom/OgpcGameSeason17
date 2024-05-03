using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarScript : MonoBehaviour
{
    public RectTransform radar;
    public GameObject posDotFab;
    public GameObject[] VoidObjects;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < VoidObjects.Length; i++)
        {

            print("terf");
            if (other == VoidObjects[i])
            {

                print("terf2");
                Vector3 objectPosition = other.gameObject.transform.position;

                UpdateRadarposition(objectPosition);
            }
        }
    }

    private void UpdateRadarposition(Vector3 objectPosition) 
    {
        print("terf");
        Vector3 center = radar.position;
        Vector3 relativePosition = objectPosition - center;
        Vector3 radarPos = new Vector2(relativePosition.x, relativePosition.y);


        GameObject posDot = Instantiate(posDotFab, radar);

        Image posDotImage = posDot.GetComponent<Image>();
        posDotImage.rectTransform.anchoredPosition = radarPos;
    }
}
