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
            totalDist *= -1;
        }
        stepCount = Mathf.RoundToInt(totalDist);

        Vector3 direction = Vector3.Normalize(startingPointObj.transform.position - endingPointObj.transform.position);
        for (int i = 1; i <= stepCount; i++)
        {

            Vector3 offset = startingPointObj.transform.position + direction * distance * i;
            Instantiate(printFab, startingPointObj.transform.position + offset, printFab.transform.rotation, parentObj.transform);
        }
    }
}
