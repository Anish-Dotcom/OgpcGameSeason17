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

    
    void Update()
    {
        InvokeRepeating("Mapdots",0,1);
    }

    
    void Mapdots()
    {
        for (int i = 0; i < detectable.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, detectable[i].transform.position);
            print(distance.ToString() + detectable[i]);
            if(distance<10)
            {
                Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 objPos = new Vector2(detectable[i].transform.position.x, detectable[i].transform.position.z);
                Vector2 reletivePosition = playerPos - objPos;
                Vector2 dotposition = reletivePosition * 5;
            }

        }
    }
    
}
