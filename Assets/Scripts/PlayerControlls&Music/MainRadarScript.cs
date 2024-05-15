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
    public RectTransform swiper;
    
    void Start()
    {
        InvokeRepeating("Mapdots",0,01f);
    }


    void Update()
    {
        Vector3 center = radarUI.position;
        swiper.RotateAround(center,Vector3.forward, 100*Time.deltaTime);
        if (RectOverlaps(radarDotPrefab, swiper))
        {
            Debug.Log("UI elements are overlapping!");
            // Do something when the UI elements overlap
        }
    }
    
    public void Mapdots()
    {
        foreach (Transform child in radarUI.transform)
        {
            if (child != swiper)
            {
                Destroy(child.gameObject);
            }
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
    private bool RectOverlaps(Rect rect1, Rect rect2)
    {
        return rect1.Overlaps(rect2);
    }
}
