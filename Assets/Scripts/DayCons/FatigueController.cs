using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public int iter = 0;
    public float fatigue = 0    ;
    public GameObject shadowBorder;
    public DissolveController DissolveController;
    public bedScript bedScript;
    public GameObject transparent;
    float x;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OutputTime", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            fatigue+=0.01f;
            transform.hasChanged = false;
        }
        
        if(fatigue> 10) {

           


            print("sleepy");
            //StartCoroutine(transparentUp());
            //bedScript.sleepy = true;
            StartCoroutine(Cutoff());
            fatigue = 0;
        }

        
    }
    void OutputTime() {
        fatigue++;
    }

    IEnumerator transparentUp()
    {
        

        yield return new WaitForSeconds(0.5f);
        iter++;
        if (iter <= 100)
        {
            StartCoroutine(transparentUp());
        }
    }

    IEnumerator Cutoff ()
    {
        

        print("cutoff");
        Vector3 cutDistanceChanges = new Vector3(fatigue/2000, 0, fatigue/1000);
        DissolveController.SetCutoffDist(cutDistanceChanges);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Cutoff());
        
    }
    public void resetTransparency ()
    {
        Vector3 cutDistanceChanges = new Vector3(5, 4, 10);
        DissolveController.SetCutoffDist(cutDistanceChanges);
        StopAllCoroutines();

    }
}
