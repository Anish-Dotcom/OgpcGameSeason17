using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{
    public GameObject[] assemblyPartsInScene;

    public GameObject RecipeForAssemblyObj;
    public GameObject AssemblyPartsInPosition;

    public string[] toyNamesForFinal;
    public Vector3[] toyPartPreFinalPos;//goes to this position until fully built
    public Vector3[] toyPartFinalPos; //once toy is fully built ease to here, once eased, instatiate final gameobject
    public bool[] toyPartInFinalPos;//in order for this to be okay you either have to be able to take toy parts out (dont like) or some sort of detection system for if the player wants to make an assembly (better, harder)

    public GameObject completedAssembly;
    public float timeToEaseToFinal;
    public float timePassed;
    private float storedTime = 10000000;//use for finding how much time has passed

    public GameObject emptyObj;//usingForFormating



    void Start()
    {
        assemblyPartsInScene = GameObject.FindGameObjectsWithTag("assemblyPart");//call this whenever new objects get added to scene as well
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        if (storedTime <= timePassed - timeToEaseToFinal)//after parts ease to position summon combined assembly
        {
            //clear all variables
            summonAssembly();
            storedTime = 10000000;
        }
    }

    //---
    public void summonAssembly()
    {
        for (int i = 0; i < AssemblyPartsInPosition.GetComponent<Transform>().childCount; i++)//removes old parts that finished easing to position
        {
            Destroy(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(i));
        }
        Instantiate(completedAssembly, new Vector3(0, 0, 0), Quaternion.identity);//parent
    }
    //---

    private void OnTriggerEnter(Collider col)//assembly
    {
        Debug.Log("enter");
        if (col.gameObject.CompareTag("assemblyPart"))
        {
            for (int i = 0; i < assemblyPartsInScene.Length; i++)
            {
                for (int b = 0; b < toyNamesForFinal.Length; b++)
                {
                    if (assemblyPartsInScene[i].name == toyNamesForFinal[b] && toyPartInFinalPos[i] == false)//as soon as it goes inside it does this, should need button press --------------------------- while in the place otherwise it has the corasponding outline enabled
                    {
                        Vector3 velocity = Vector3.zero;

                        assemblyPartsInScene[i].transform.position = Vector3.SmoothDamp(assemblyPartsInScene[i].transform.position, toyPartPreFinalPos[i], ref velocity, 1);
                        RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(i).gameObject.SetActive(false);//disable outline same as obj

                        assemblyPartsInScene[i].GetComponent<Transform>().SetParent(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(i).GetComponent<Transform>());//sets to be a child of the assosciated
                        toyPartInFinalPos[i] = true;

                        bool assemblyDone = true;//true unless any is false
                        for (int c = 0; c < toyPartInFinalPos.Length; c++)
                        {
                            if (!toyPartInFinalPos[c])//checking if all are true, if even one is false, return.
                            {
                                assemblyDone = false;
                            }
                        }
                        if (assemblyDone)
                        {
                            //Clear RecipeForAssemblyObj
                            Destroy(RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0));
                            storedTime = timePassed;
                        }
                        return;
                    }
                }
            }
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i > RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().childCount; i++)
            {
                if (!toyPartInFinalPos[i])
                {
                    RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(i).gameObject.SetActive(true);//enable all outlines 
                }
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        Debug.Log("exit");//only if its the player
        if (col.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i > RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().childCount; i++)
            {
                RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(i).gameObject.SetActive(false);//disable all outlines 
            }
        }
    }
    //outline
    public void SetAssembly(GameObject RecipeObj, GameObject CompletedAssembly)
    {
        completedAssembly = CompletedAssembly;
        GameObject RecipeObject = Instantiate(RecipeObj, RecipeForAssemblyObj.transform.position, Quaternion.identity, RecipeForAssemblyObj.GetComponent<Transform>());//should work, position should be inherited, if not set the world position to RecipeForAssemblyObj world pos
        toyNamesForFinal = new string[RecipeObject.transform.childCount];//get children of gameobject, each child is a part of the assembly
        toyPartPreFinalPos = new Vector3[RecipeObject.transform.childCount];
        toyPartFinalPos = new Vector3[completedAssembly.transform.childCount];

        for (int i = 0; i < RecipeObject.transform.childCount; i++)
        {
            toyNamesForFinal[i] = RecipeObject.transform.GetChild(i).name;
            toyPartPreFinalPos[i] = RecipeObject.transform.GetChild(i).transform.position;
            toyPartFinalPos[i] = CompletedAssembly.transform.GetChild(i).transform.position;
            GameObject Recent = Instantiate(emptyObj, new Vector3(0, 0, 0), Quaternion.identity, AssemblyPartsInPosition.GetComponent<Transform>());
            Recent.name = toyNamesForFinal[i] + " " + i.ToString();//setNameToSameAsToyNames + something to differentiate
        }
    }
}