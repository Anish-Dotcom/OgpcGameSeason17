using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueController : MonoBehaviour
{
    public float fatigue = 0    ;
    public GameObject shadowBorder;
    public DissolveController DissolveController;
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
        
        if(fatigue> 50) {
            CanvasGroup CG = shadowBorder.GetComponent<CanvasGroup>();
            CG.alpha += 0.01f;
            DissolveController.areaMat.SetFloat("_Cutoff_Distance_X", DissolveController.dissolveDistances.x-2);
            DissolveController.areaMat.SetFloat("_Cutoff_Distance_Z", DissolveController.dissolveDistances.z-2);
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
