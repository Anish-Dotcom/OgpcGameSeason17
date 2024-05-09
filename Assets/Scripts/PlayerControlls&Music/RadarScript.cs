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
    public float detectionRadius = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        List<GameObject> detectedObjects = new List<GameObject>();


    }

    private void OnTriggerExit (Collider other)
    {
        for (int i = 0; i < VoidObjects.Length; i++)
        {

            print("ARKER");
            if (other == VoidObjects[i])
            {
                Vector3 objectPosition = other.gameObject.transform.position;

                UpdateRadarposition(objectPosition);
            }
        }
    }

    private void UpdateRadarposition(Vector3 objectPosition) 
    {
        Vector3 center = radar.position;
        Vector3 relativePosition = objectPosition - center;
        Vector3 radarPos = new Vector2(relativePosition.x, relativePosition.y);


        GameObject posDot = Instantiate(posDotFab, radar);

        Image posDotImage = posDot.GetComponent<Image>();
        posDotImage.rectTransform.anchoredPosition = radarPos;
    }
}
