using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{
    public static GameObject[] assemblyPartsInScene;

    public GameObject RecipeForAssemblyObj;
    public GameObject AssemblyPartsInPosition;

    public string[] toyNamesForFinal;
    public Vector3[] toyPartFinalPos;
    public Vector3 assemblyOutlineLocaiton;
    public bool[] toyPartInFinalPos;//in order for this to be okay you either have to be able to take toy parts out (dont like) or some sort of detection system for if the player wants to make an assembly (better, harder)
    public GameObject completedAssembly;

    public GameObject emptyObj;//usingForFormating


    void Start()
    {
        assemblyPartsInScene = GameObject.FindGameObjectsWithTag("assemblyPart");//call this whenever new objects get added to scene as well
    }
    void Update()
    {

    }

    //---
    public void summonAssembly()
    {
        Instantiate(completedAssembly, new Vector3(0, 0, 0), Quaternion.identity);//parent
    }
    //---

    private void OnTriggerEnter(Collider col)//assembly
    {
        if (col.gameObject.CompareTag("assemblyPart"))
        {
            for (int i = 0; i < assemblyPartsInScene.Length; i++)
            {
                for (int b = 0; b < toyNamesForFinal.Length; i++)
                {
                    if (assemblyPartsInScene[i].name == toyNamesForFinal[b] && toyPartInFinalPos[i] == false) 
                    {
                        assemblyPartsInScene[i].transform.position = toyPartFinalPos[i];//make ease to position later, just sets straight to position
                        assemblyPartsInScene[i].GetComponent<Transform>().SetParent(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(i).GetComponent<Transform>());//sets to be a child of the assosciated
                        toyPartInFinalPos[i] = true;

                        bool assemblyDone = true;//true unless any is false
                        for (int c = 0; c < toyPartInFinalPos.Length; c++)
                        {
                            if (!toyPartInFinalPos[c])//checking if all are true, if even one is false, return.
                            {
                                assemblyDone = false;
                                return;
                            }
                        }
                        if (assemblyDone)
                        {
                            summonAssembly();
                        }
                        return;
                    }
                }
            }
        }
    }
    public void SetAssembly(GameObject OutlineObjectRef)//have a gameobject which is of the completed toy part but with the hallow material and unpickupable + no grav, just transform and mesh. This means we can highlight the position the part will be moved to + edit it easier
    {
        GameObject OutlineObject = Instantiate(OutlineObjectRef, assemblyOutlineLocaiton, Quaternion.identity, RecipeForAssemblyObj.GetComponent<Transform>());
        toyNamesForFinal = new string[OutlineObject.transform.childCount];//get children of gameobject, each child is a part of the assembly
        toyPartFinalPos = new Vector3[OutlineObject.transform.childCount];

        for (int i = 0; i < OutlineObject.transform.childCount; i++)
        {
            toyNamesForFinal[i] = OutlineObject.transform.GetChild(i).name;
            toyPartFinalPos[i] = OutlineObject.transform.GetChild(i).transform.position;
            GameObject Recent = Instantiate(emptyObj, new Vector3(0, 0, 0), Quaternion.identity, AssemblyPartsInPosition.GetComponent<Transform>());
            Recent.name = toyNamesForFinal[i] + " " + i.ToString();//setNameToSameAsToyNames + something to differentiate
        }
    }
}
