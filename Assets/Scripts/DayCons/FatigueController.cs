using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public int iter = 0;
    public float fatigue = 1    ;
    public float fatugueUnchanged = 0;
    public GameObject shadowBorder;
    public DissolveController DissolveController;
    public bedScript bedScript;
    public GameObject transparent;
    float x;
    float z;
    public Vector3 DissolveSpeed;
    public float dayTime;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OutputTime", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //If moved add fatigue
        if (transform.hasChanged)
        {
            fatigue+=0.01f;
            fatugueUnchanged += 0.01f;
            transform.hasChanged = false;
        }
        
        if(fatigue> dayTime) {//If fatigue exeeds day time start croutine that moves in darkness
        
            fatigue = 0;//Resets fatigue
        }

        Vector3 cutDistanceChanges = new Vector3(DissolveSpeed.x / fatigue, DissolveSpeed.y / fatigue, DissolveSpeed.z / fatigue);
        DissolveController.SetCutoffDist(cutDistanceChanges);
    }
    void OutputTime() {
        fatigue++;
        fatugueUnchanged++;//Keeps original fatigue
    }

    //IEnumerator transparentUp()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    iter++;
    //    if (iter <= 100)
    //    {
    //        StartCoroutine(transparentUp());
    //    }
    //}

    IEnumerator CutoffOff ()
    {//Begins a cutoff
        print("cutoff");
        Vector3 cutDistanceChanges = new Vector3(fatugueUnchanged/800000, fatugueUnchanged / 700000, fatugueUnchanged / 400000);
        DissolveController.SetCutoffDist(cutDistanceChanges);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(CutoffOff());
        
    }
    public void Cutoff()
    {
        Vector3 cutDistanceChanges = new Vector3(80000 / fatigue, 70000 / fatigue, fatigue / 40000);
        DissolveController.SetCutoffDist(cutDistanceChanges);
    }
    public void resetTransparency ()
    {//Resets transpareny referenced in bedscript
        Vector3 cutDistanceChanges = new Vector3(5, 4, 10);
        fatugueUnchanged = 0;
        fatigue = 1;
        DissolveController.setCutoffNoChange(cutDistanceChanges);
        StopAllCoroutines();

    }
}
