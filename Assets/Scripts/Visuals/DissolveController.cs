using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Vector3 dissolveDistances;//The distance in radius around the centralPos that things will apear
    public Vector3 startingDissolveDistances;//whatever dissolve distances are first at runtime

    public GameObject centralObj;//able to change positon to cleanly make something appear
    public Vector3 centralPos;//position central obj starts at

    public List<Material> areaMats;
    public List<Material> dualDissolveAreaMats;
    public List<Material> updatingMats;
    public List<Material> dualDissolveUpdatingMats;
    public float updatedRadius;
    public float startingRadius;

    public float[] size;//0, is outer size, 1 is inner
    public GameObject[] objsToEnable;

    public float dissolveNoiseStrength;

    void Start()
    {
        dissolveDistances = startingDissolveDistances;
        centralPos = centralObj.transform.position;
        for (int i = 0; i < areaMats.Count; i++)
        {
            areaMats[i].SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
            //Debug.Log(areaMats[i].GetVector("_Object_Position_For_Ref_Dis"));
            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
            areaMats[i].SetFloat("_Noise_Strength", dissolveNoiseStrength);
        }
        for (int i = 0; i < dualDissolveAreaMats.Count; i++)//for mats with two layers of dissolve
        {
            dualDissolveAreaMats[i].SetVector("_Obj_Pos_non_move", centralObj.transform.position);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_X_non_move", dissolveDistances.x);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Y_non_move", dissolveDistances.y);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Z_non_move", dissolveDistances.z);

            dualDissolveAreaMats[i].SetFloat("_Cutoff_Radius", updatedRadius);
            dualDissolveAreaMats[i].SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
        }
    }
    void Update()
    {
        if (updatingMats.Count > 0)
        {
            SetObjPos(updatingMats);
        }
        if (dualDissolveUpdatingMats.Count > 0)
        {
            setSecondDissolveCenter(dualDissolveUpdatingMats);
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
    public void SetObjPos(List<Material> updating, Vector3 position)
    {
        for (int i = 0; i < updating.Count; i++)
        {
            updating[i].SetVector("_Object_Position_For_Ref_Dis", position);
            //Debug.Log(updating[i].GetVector("_Object_Position_For_Ref_Dis"));
        }
    }
    public void SetCutoffDist(Vector3 distanceChanges)
    {
        for (int i = 0; i < areaMats.Count; i++)
        {
            dissolveDistances -= distanceChanges;

            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);

            /*Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_X"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Y"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Z"));*/
        }
        for (int i = 0; i < dualDissolveAreaMats.Count; i++)
        {
            dualDissolveAreaMats[i].SetFloat("_Cutoff_X_non_move", dissolveDistances.x);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Y_non_move", dissolveDistances.y);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Z_non_move", dissolveDistances.z);
        }
    }
    public void setCutoffNoChange(Vector3 distanceChanges)
    {
        for (int i = 0; i < areaMats.Count; i++)
        {
            dissolveDistances = distanceChanges;

            areaMats[i].SetFloat("_Cutoff_Distance_X", dissolveDistances.x);  
            areaMats[i].SetFloat("_Cutoff_Distance_Y", dissolveDistances.y);
            areaMats[i].SetFloat("_Cutoff_Distance_Z", dissolveDistances.z);
            
            /*Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_X"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Y"));
            Debug.Log(areaMats[i].GetFloat("_Cutoff_Distance_Z"));*/
        }
        for (int i = 0; i < dualDissolveAreaMats.Count; i++)
        {
            dualDissolveAreaMats[i].SetFloat("_Cutoff_X_non_move", dissolveDistances.x);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Y_non_move", dissolveDistances.y);
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Z_non_move", dissolveDistances.z);
        }
    }
    public void setSecondDissolveRadius(float radius)//for player dissovle
    {
        updatedRadius = radius;
        for (int i = 0; i < dualDissolveAreaMats.Count; i++)
        {
            dualDissolveAreaMats[i].SetFloat("_Cutoff_Radius", radius);
            //Debug.Log(dualDissolveAreaMats[i].GetFloat("_Cutoff_Radius") + " " + gameObject.name);
        }
    }
    public void setSecondDissolveCenter(List<Material> updating)
    {
        for (int i = 0; i < updating.Count; i++)
        {
            updating[i].SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
        }
    }
    public void setSecondDissolveCenter(List<Material> updating, Vector3 position)
    {
        for (int i = 0; i < updating.Count; i++)
        {
            updating[i].SetVector("_Object_Position_For_Ref_Dis", position);
        }
    }
    public void objNonMoveCenterSet(List<Material> updating)
    {
        centralPos = centralObj.transform.position;
        for (int i = 0; i < updating.Count; i++)
        {
            //Debug.Log(updating[i].GetVector("_Obj_Pos_non_move") + " " + gameObject.name);
            updating[i].SetVector("_Obj_Pos_non_move", centralObj.transform.position);
            //Debug.Log(updating[i].GetVector("_Obj_Pos_non_move") + " " + gameObject.name);
        }
    }
    public void BoxUpdate(Material boxmat)
    {
        boxmat.SetVector("_Obj_Pos_non_move", centralObj.transform.position);
        boxmat.SetVector("_Object_Position_For_Ref_Dis", centralObj.transform.position);
    }
    public void BoxSet(Material boxmat)
    {
        boxmat.SetFloat("_Cutoff_X_non_move", dissolveDistances.x);
        boxmat.SetFloat("_Cutoff_Y_non_move", dissolveDistances.y);
        boxmat.SetFloat("_Cutoff_Z_non_move", dissolveDistances.z);

        boxmat.SetFloat("_Cutoff_Radius", startingRadius);
    }
}