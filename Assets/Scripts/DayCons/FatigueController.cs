using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public int z = 0;
    public float fatigue = 0    ;
    public GameObject shadowBorder;
    public DissolveController DissolveController;
    public bedScript bedScript;
    public GameObject transparent;
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
        
        if(fatigue> 10) {
            StartCoroutine(transparentUp());
            transparent.GetComponent<CanvasGroup>().alpha += 0.1f;
            bedScript.sleepy = true;
            StartCoroutine(Cutoff());
        }

        
    }
    void OutputTime() {
        fatigue++;
    }

    IEnumerator transparentUp()
    {
        
        transparent.GetComponent<CanvasGroup>().alpha += 0.00000015f;
        yield return new WaitForSeconds(0.5f);
        z++;
        if (z <= 100)
        {
            StartCoroutine(transparentUp());
        }
    }

    IEnumerator Cutoff ()
    {
        Vector3 cutDistanceChanges = new Vector3(-1, 0, -3);
        DissolveController.SetCutoffDist(cutDistanceChanges);
        yield return new WaitForSeconds(1);
        StartCoroutine(Cutoff());
    }
}
