using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public float dissolveDistance;//The distance in radius around the centralPos that things will apear
    public GameObject centralObj;//able to change positon to cleanly make something appear
    public Material areaMat;

    void Start()
    {
        areaMat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
        areaMat.SetFloat("_Cutoff_Distance", dissolveDistance);
    }
    void Update() 
    {
        areaMat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
    }
    public void SetObjPos()
    {
        areaMat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
    }
    public void SetCutoffDist() 
    {
        areaMat.SetFloat("_Cutoff_Distance", dissolveDistance);
    }
}