using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRadarScript : MonoBehaviour
{
    public Transform centerObject; 
    public GameObject radarDotPrefab;
    public RectTransform radarUI; 
    public float maxDistance;
    public GameObject[] detectable;
    public int radarInt;

    
    void Start()
    {
        InvokeRepeating("Mapdots",0,1);

    }

    
    void Mapdots()
    {
        foreach (Transform child in radarUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject obj in detectable)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < maxDistance)
            {
                Vector3 playerPos = new Vector3(transform.position.x, transform.position.z, 0);
                Vector3 objPos = new Vector3(obj.transform.position.x, obj.transform.position.z, 0);
                Vector3 relativePosition = objPos - playerPos;
                Vector2 dotPosition = new Vector2(relativePosition.x, relativePosition.y);

                dotPosition /= maxDistance;
                dotPosition *= radarUI.sizeDelta.x / 2;

                GameObject radarDot = Instantiate(radarDotPrefab, radarUI);
                radarDot.GetComponent<RectTransform>().anchoredPosition = dotPosition;
                
            }
        }

    }

}
