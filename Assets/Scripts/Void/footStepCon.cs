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

        Vector3 direction = Vector3.Normalize(startingPointObj.transform.position - endingPointObj.transform.position);
        //float angle = - - - 
        //Debug.Log(angle);

        for (int i = 1; i <= stepCount; i++)
        {
            if (i % 2 == 0)//even i
            {
                //Vector3 angleFlip = new Vector3(, , );

            }
            else //odd i
            {

            }
            Vector3 offset = startingPointObj.transform.position + direction * distance * i;

            Vector3 position = new Vector3(startingPointObj.transform.position.x + offset.x, printFab.transform.position.y, startingPointObj.transform.position.z + offset.z);
            
            //Instantiate(printFab, position, rotation, parentObj.transform);
        }
    }
}
