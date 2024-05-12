using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRadarScript : MonoBehaviour
{
    public Transform centerObject; 
    public GameObject radarDotPrefab;
    public RectTransform radarUI; 
    public float maxDistance;

    
    void Update()
    {
        
        CalculateAndMapRadarDots();
    }

    void CalculateAndMapRadarDots()
    {
       
        ClearRadarDots();

        GameObject[] objectsToTrack = GameObject.FindGameObjectsWithTag("Detectable");

        foreach (GameObject obj in objectsToTrack)
        {
            Vector3 relativePosition = obj.transform.position - centerObject.position;

            float angle = Mathf.Atan2(relativePosition.z, relativePosition.x) * Mathf.Rad2Deg;
            float distance = relativePosition.magnitude;

            float radarRadius = Mathf.Min(radarUI.rect.width, radarUI.rect.height) / 2f;
            float normalizedDistance = distance / maxDistance; 
            Vector2 radarPosition = new Vector2(Mathf.Cos(angle) * radarRadius * normalizedDistance, Mathf.Sin(angle) * radarRadius * normalizedDistance);

            GameObject radarDot = Instantiate(radarDotPrefab, radarPosition, Quaternion.identity, radarUI);
            print("didthat");
        }
    }

    void ClearRadarDots()
    {
        foreach (Transform child in radarUI)
        {
            Destroy(child.gameObject);
        }
    }
}
