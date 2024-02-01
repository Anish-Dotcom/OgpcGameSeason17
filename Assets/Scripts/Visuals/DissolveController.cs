using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Vector3 dissolveDistances;//The distance in radius around the centralPos that things will apear
    public GameObject centralObj;//able to change positon to cleanly make something appear
    public Material areaMat;

    void Update()
    {
        
    }
    void Start()
    {
        areaMat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
        Debug.Log(areaMat.GetVector("_Object_Position_For_Ref_Dis"));
        areaMat.SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
        areaMat.SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
        areaMat.SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
    }
    public void SetObjPos()
    {
        areaMat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
    }
    public void SetCutoffDist() 
    {
        areaMat.SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
        areaMat.SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
        areaMat.SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
    }
}