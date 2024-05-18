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
    public float prevRot;
    float oldAOR =0;


    void Start()
    {

    }


    void Update()
    {
        
        Vector3 center = radarUI.position;
        swiper.RotateAround(center,Vector3.forward, 100*Time.deltaTime);

        Mapdots();
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
                Vector2 playerPos = new Vector3(transform.position.x, transform.position.z);
                Vector2 objPos = new Vector3(obj.transform.position.x, obj.transform.position.z);
                Vector2 relativePosition = objPos - playerPos;
                Vector2 dotPosition = new Vector2(relativePosition.x, relativePosition.y);

                float referanceAngle = Mathf.Atan(relativePosition.y/relativePosition.x);
                float angleOfRotation = 0f;
                if (relativePosition.x > 0 && relativePosition.y > 0)
                {
                    angleOfRotation = referanceAngle;
                }
                else if (relativePosition.x < 0 && relativePosition.y > 0)
                {
                    angleOfRotation = Mathf.PI - referanceAngle;
                }
                else if (relativePosition.x < 0 && relativePosition.y < 0)
                {
                    angleOfRotation = Mathf.PI + Mathf.Abs(referanceAngle);
                }
                else if (relativePosition.x > 0 && relativePosition.y < 0)
                {
                    angleOfRotation = (Mathf.PI * 2) - Mathf.Abs(referanceAngle);
                }
                float swiperRotation = NormalizeAngle(swiper.eulerAngles.z * Mathf.Deg2Rad);



                print(angleOfRotation);
                print(oldAOR);
                print(swiperRotation);
                print("");
                if (swiperRotation>oldAOR-1&&swiperRotation<angleOfRotation+1)
                {
                    dotPosition /= maxDistance;
                    dotPosition *= radarUI.sizeDelta.x / 2;

                    GameObject radarDot = Instantiate(radarDotPrefab, radarUI);
                    radarDot.GetComponent<RectTransform>().anchoredPosition = dotPosition;
                }

                oldAOR = angleOfRotation;
            }
        }

    }
    private float NormalizeAngle(float angle)
    {
        // Normalize an angle to be between 0 and 2*PI
        while (angle < 0) angle += 2 * Mathf.PI;
        while (angle > 2 * Mathf.PI) angle -= 2 * Mathf.PI;
        return angle;
    }
    
}
