using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{
    public static GameObject[] assemblyPartsInScene;

    public GameObject parentAssemblyObj;
    public string[] toyNamesForFinal;
    public Vector3[] toyPartFinalPos;
    public bool[] toyPartInFinalPos;//in order for this to be okay you either have to be able to take toy parts out (dont like) or some sort of detection system for if the player wants to make an assembly (better, harder)
    public GameObject completedAssembly;


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
                        assemblyPartsInScene[i].transform.position = toyPartFinalPos[i];//make ease to position
                        toyPartInFinalPos[i] = true;

                        bool assemblyDone = true;
                        for (int c = 0; c < toyPartInFinalPos.Length; c++)
                        {
                            if (!toyPartInFinalPos[c]) 
                            {
                                assemblyDone = false;
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
}
