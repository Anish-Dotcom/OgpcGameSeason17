using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footStepCon : MonoBehaviour
{
    public GameObject printFab;//foot print prefab
    public GameObject parentObj;//obj footprints are parented to

    public GameObject startingPointObj;//point where footprints start
    public GameObject endingPointObj;//point where they end

    public float distance;//distance between footprints
    public float sideSpacing;//distance to the first step - distance (side spacing + distance = distance to first step)
    public int stepCount;//number of steps

    public float upperBoundSpacing;//distance between steps max (width)
    public float lowerBoundSpacing;//distance between steps min (width)

    public Material footPrintMat;//original footprint mat

    public List<Material> footPrintMatsInScene;//each different color of footprint requires a different material

    // Start is called before the first frame update
    void Start()
    {
        setFootPrints(new Color(255,0,0));//red
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFootPrints(Color color)
    {
        Material newPrintMat = new Material(footPrintMat);
        newPrintMat.color = color;
        footPrintMatsInScene.Add(newPrintMat);

        float totalDist = Vector3.Distance(startingPointObj.transform.position, endingPointObj.transform.position);
        if (totalDist < 0)
        {
            totalDist *= -1;//make positive
        }
        stepCount = Mathf.RoundToInt(totalDist/distance);//calculates the number of footsteps based on distance between locations divided by distance between steps

        Vector3 direction = Vector3.Normalize(endingPointObj.transform.position - startingPointObj.transform.position);//calculates the direction from starting point to ending point with a magnitude of 1.
        //Debug.DrawRay(startingPointObj.transform.position, direction, Color.green, 100f);

        Vector3 perpendicularDirection = Vector3.Normalize(new Vector3(startingPointObj.transform.position.z - endingPointObj.transform.position.z, 0, endingPointObj.transform.position.x - startingPointObj.transform.position.x));//creates the perpendicular vector to direction
        //Debug.DrawRay(startingPointObj.transform.position, perpendicularDirection, Color.red, 100f);

        float angle = Mathf.Atan(direction.x/direction.z) * (180/Mathf.PI);//calculates the angle the foot needs to face to aim towards ending point obj
        Quaternion rotation;
        Vector3 perpOffset;

        for (int i = 1; i <= stepCount; i++)
        {
            if (i % 2 == 0)//even i, if its even flip foot 180 deg, 
            {
                rotation = Quaternion.Euler(-90, angle, 180 + Random.Range(-5f, 5f));
                perpOffset = perpendicularDirection * Random.Range(-0.2f, -1f);
            }
            else //odd i
            {
                rotation = Quaternion.Euler(90, angle, 0 + Random.Range(-5f, 5f));
                perpOffset = perpendicularDirection * Random.Range(0.2f, 1f);
            }


            Vector3 offset = perpOffset + (direction * distance * i);
            Vector3 position = new Vector3(startingPointObj.transform.position.x + offset.x, printFab.transform.position.y, startingPointObj.transform.position.z + offset.z);

            Instantiate(printFab, position, rotation, parentObj.transform);
        }
    }
}