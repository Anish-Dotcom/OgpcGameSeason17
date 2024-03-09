using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footStepCon : MonoBehaviour
{
    public GameObject printFab;
    public GameObject parentObj;

    public GameObject startingPointObj;
    public GameObject endingPointObj;

    public float distance;//distance between footprints
    public int stepCount;//number of steps

    // Start is called before the first frame update
    void Start()
    {
        setFootPrints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFootPrints()
    {
        float totalDist = Vector3.Distance(startingPointObj.transform.position, endingPointObj.transform.position);
        if (totalDist < 0)
        {
            totalDist *= -1;//make positive
        }
        stepCount = Mathf.RoundToInt(totalDist/distance);

        Vector3 direction = Vector3.Normalize(endingPointObj.transform.position - startingPointObj.transform.position);//--
        Debug.Log(direction);

        float angle = Mathf.Atan(direction.x/direction.z) * (180/Mathf.PI);//calculates the angle the foot needs to face to aim towards ending point obj
        Quaternion rotation = Quaternion.Euler(90, angle, 0);

        for (int i = 1; i <= stepCount; i++)
        {
            if (i % 2 == 0)//even i, if its even flip foot 180 deg, 
            {
                rotation = Quaternion.Euler(-90, angle, 180);

            }
            else //odd i
            {
                rotation = Quaternion.Euler(90, angle, 0);
            }
            Vector3 offset = direction * distance * i;//--

            Vector3 position = new Vector3(startingPointObj.transform.position.x + offset.x, printFab.transform.position.y, startingPointObj.transform.position.z + offset.z);
            Instantiate(printFab, position, rotation, parentObj.transform);
        }
    }
}