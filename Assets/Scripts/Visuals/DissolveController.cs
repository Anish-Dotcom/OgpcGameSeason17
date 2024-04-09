using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Vector3 dissolveDistances;//The distance in radius around the centralPos that things will apear
    public Vector3 startingDissolveDistances;//whatever dissolve distances are first at runtime

    public GameObject centralObj;//able to change positon to cleanly make something appear
    public Vector3 centralPos;//position central obj starts at

    public Material[] areaMats;
    public Material[] dualDissolveAreaMats;
    public Vector3 nonMoveDissolveDistances;
    public List<Material> updatingMats;

    public float[] size;//0, is outer size, 1 is inner
    public GameObject[] objsToEnable;

    public float dissolveNoiseStrength;

    void Start()
    {
        dissolveDistances = startingDissolveDistances;
        centralPos = centralObj.transform.position;
        for (int i = 0; i < areaMats.Length; i++)
        {
            areaMats[i].SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
            //Debug.Log(areaMats[i].GetVector("_Object_Position_For_Ref_Dis"));
            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
            areaMats[i].SetFloat("_Noise_Strength", dissolveNoiseStrength);
        }
        for (int i = 0; i < dualDissolveAreaMats.Length; i++)//for mats with two layers of dissolve
        {
            dualDissolveAreaMats[i].SetVector("_Obj_Pos_non_move", centralObj.transform.position);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_X_non_move", nonMoveDissolveDistances.x);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_y_non_move", nonMoveDissolveDistances.y);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_z_non_move", nonMoveDissolveDistances.z);
        }
    }
    void Update()
    {
        if (updatingMats.Count > 0)
        {
            SetObjPos(updatingMats);
        }
    }
    public void SetObjPos(List<Material> updating)
    {
        for (int i = 0; i < updating.Count; i++)
        {
            updating[i].SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
            //Debug.Log(updating[i].GetVector("_Object_Position_For_Ref_Dis"));
        }
    }
    public void SetCutoffDist(Vector3 distanceChanges)
    {
        for (int i = 0; i < areaMats.Length; i++)
        {
            dissolveDistances -= distanceChanges;

            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);

            /*Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_X"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Y"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Z"));*/
        }
    }
    public void setCutoffNoChange(Vector3 distanceChanges)
    {
        for (int i = 0; i < areaMats.Length; i++)
        {
            dissolveDistances = distanceChanges;

            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);  
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
            
            /*Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_X"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Y"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Z"));*/
        }
    }
}