using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footStepCon : MonoBehaviour
{
    public GameObject printFab;
    public GameObject startingPointObj;
    public GameObject endingPointObj;
    public float distanceBetweenSteps;

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
        Vector3 totalDist = startingPointObj.transform.position - endingPointObj.transform.position;
        Debug.Log(totalDist);
    }
}
