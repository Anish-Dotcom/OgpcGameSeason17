using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public float fatigue = 0    ;
    public GameObject shadowBorder;
    public DissolveController DissolveController;
    public bedScript bedScript;
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
            //print(fatigue);
            transform.hasChanged = false;
        }
        
        if(fatigue> 50) {
            bedScript.sleepy = true;
            StartCoroutine(Cutoff());
        }

        
    }
    void OutputTime() {
        fatigue++;
    }

    IEnumerator Cutoff ()
    {
        DissolveController.areaMat.SetFloat("_Cutoff_Distance_X", DissolveController.dissolveDistances.x - 1);
        DissolveController.areaMat.SetFloat("_Cutoff_Distance_Z", DissolveController.dissolveDistances.z - 3);
        yield return new WaitForSeconds(1);
        StartCoroutine(Cutoff());
    }
}
