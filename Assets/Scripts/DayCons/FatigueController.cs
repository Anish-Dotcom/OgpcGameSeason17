using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public float fatigue = 0;
    public GameObject shadowBorder;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            fatigue+=0.001f;
            //print(fatigue);
            transform.hasChanged = false;
        }
        
        if(fatigue> 500) {
            CanvasGroup CG = shadowBorder.GetComponent<CanvasGroup>();
            CG.alpha += 0.01f;
        }

        
    }

    IEnumerator Timer()
    {
        fatigue++;
        //print(fatigue);
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }
}
