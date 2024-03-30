using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{

    public GameObject assembly;
    public GameObject finalObj;

    //above are for testing

    public Material recipeMat;

    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public float distFromPlayer;
    public GameObject playerCam;
    public GameObject heldObjContainer;

    public GameObject[] assemblyPartsInScene;

    public GameObject RecipeForAssemblyObj;
    public GameObject AssemblyPartsInPosition;

    public string[] toyNamesForFinal;
    public bool[] toyPartInFinalPos;//in order for this to be okay you either have to be able to take toy parts out (dont like) or some sort of detection system for if the player wants to make an assembly (better, harder)

    public GameObject completedAssembly;
    public float timeToEaseToFinal;
    public float timePassed;
    private float storedTime = 10000000;//use for finding how much time has passed

    public GameObject emptyObj;//usingForFormating

    //smoothdamp variables:
    public GameObject[] objectsBeingMoved;
    public float[] timeBeenMoved;
    public float easeSpeedPreFinal = 0.025f;//lower numbers coraspond to faster movement
    public float easeSpeedFinal = 0.04f;//lower numbers coraspond to faster movement



    void Start()
    {
        assemblyPartsInScene = GameObject.FindGameObjectsWithTag("assemblyPart");//call this whenever new objects get added to scene as well
        objectsBeingMoved = new GameObject[assemblyPartsInScene.Length];
        timeBeenMoved = new float[assemblyPartsInScene.Length];

        //for testing:
        //SetAssembly(assembly, finalObj);
        for (int i = 0; i < assemblyPartsInScene.Length; i++)
        {
            //AddToAssembly(assemblyPartsInScene[i]);
        }
    }
    void Update()
    {
        if (RecipeForAssemblyObj.GetComponent<Transform>().childCount > 0)
        {
            distFromPlayer = Vector3.Distance(playerCam.transform.position, transform.position);
            if (distFromPlayer <= 3.8)//controls transparency
            {
                RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
                recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer));
            }
            else if (distFromPlayer <= 3.9)
            {
                RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
                recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer * 1.5f));
            }
            else if (distFromPlayer <= 4)
            {
                RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
                recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer * 2));
            }
            else if (distFromPlayer > 4)
            {
                RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && lookingAtCheck.lookingAt[myInfoIndex])//add the object command
        {
            AddToAssembly(heldObjContainer.GetComponent<Transform>().GetChild(0).gameObject);
            ObjectPickUp.equipped = false;
            ObjectPickUp.slotFull = false;
        }

        Vector3[] velocity = new Vector3[objectsBeingMoved.Length];
        for (int i = 0; i < objectsBeingMoved.Length; i++)
        {
            if (objectsBeingMoved[i] != null)
            {
                if (objectsBeingMoved[i].transform.position != RecipeForAssemblyObj.transform.GetChild(0).GetChild(i).position)
                {
                    velocity[i] = objectsBeingMoved[i].GetComponent<Rigidbody>().velocity;
                    timeBeenMoved[i] += Time.deltaTime;
                    if (timeBeenMoved[i] < 1 && storedTime == 10000000)
                    {
                        objectsBeingMoved[i].transform.position = Vector3.SmoothDamp(objectsBeingMoved[i].transform.position, RecipeForAssemblyObj.transform.GetChild(0).GetChild(i).position, ref velocity[i], easeSpeedPreFinal);
                    }
                    else if (storedTime == 10000000)
                    {
                        objectsBeingMoved[i].transform.position = RecipeForAssemblyObj.transform.GetChild(0).GetChild(i).position;
                    }
                }
                else //in position, check if all are done
                {
                    Debug.Log("in position");
                    RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(i).gameObject.SetActive(false);//disable outline same as obj

                    bool assemblyDone = true;//true unless any is false
                    for (int c = 0; c < toyPartInFinalPos.Length; c++)
                    {
                        if (!toyPartInFinalPos[c])//checking if all are true, if even one is false, return.
                        {
                            assemblyDone = false;
                        }
                    }
                    if (assemblyDone && storedTime == 10000000)
                    {
                        //Clear RecipeForAssemblyObj
                        storedTime = timePassed;
                    }
                }
            }
        }

        timePassed += Time.deltaTime;
        if (storedTime <= timePassed - timeToEaseToFinal)//after parts ease to position summon combined assembly
        {
            //clear all variables
            summonAssembly();
            storedTime = 10000000;
        }
        else if (storedTime <= timePassed - timeToEaseToFinal + 3f)
        {
            for (int i = 0; i < AssemblyPartsInPosition.transform.childCount; i++)
            {
                if (AssemblyPartsInPosition.transform.GetChild(i).GetChild(0).position != completedAssembly.transform.GetChild(i).position)
                {
                    velocity[i] = AssemblyPartsInPosition.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Rigidbody>().velocity;
                    AssemblyPartsInPosition.transform.GetChild(i).GetChild(0).position = Vector3.SmoothDamp(AssemblyPartsInPosition.transform.GetChild(i).GetChild(0).position, completedAssembly.transform.GetChild(i).position + RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).position, ref velocity[i], easeSpeedFinal);
                }
            }
        }
    }

    //---
    public void summonAssembly()
    {
        Vector3 position = RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).position;
        for (int i = 0; i < AssemblyPartsInPosition.GetComponent<Transform>().childCount; i++)
        {
            Destroy(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(i).gameObject);//destroy all previous parts
        }
        Destroy(RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).gameObject);//Destroy previous assembly
        GameObject completed = Instantiate(completedAssembly, position + completedAssembly.transform.position, Quaternion.identity);//parent
    }
    //---

    private void AddToAssembly(GameObject objToAdd)//works
    {
        Debug.Log("addItemFunctionCalled" + timePassed);
        if (objToAdd.CompareTag("assemblyPart"))
        {
            for (int b = 0; b < toyNamesForFinal.Length; b++)
            {
                if (objToAdd.name == toyNamesForFinal[b] && toyPartInFinalPos[b] == false)
                {
                    objToAdd.gameObject.layer = 2;
                    ChangeColliderTrigger(objToAdd, false);

                    objToAdd.GetComponent<Rigidbody>().isKinematic = true;
                    Vector3 velocity = objToAdd.GetComponent<Rigidbody>().velocity;

                    objToAdd.GetComponent<Transform>().SetParent(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(b).GetComponent<Transform>());//sets to be a child of the assosciated
                    objToAdd.transform.rotation = RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(b).transform.rotation;


                    objectsBeingMoved[b] = objToAdd;//start smoothdamp
                    timeBeenMoved[b] = 0;
                    toyPartInFinalPos[b] = true;
                    return;
                }
            }
        }
    }

    //outline
    public void SetAssembly(GameObject RecipeObj, GameObject CompletedAssembly)//works
    {
        completedAssembly = CompletedAssembly;
        GameObject RecipeObject = Instantiate(RecipeObj, RecipeForAssemblyObj.transform.position + RecipeObj.transform.position, Quaternion.identity, RecipeForAssemblyObj.GetComponent<Transform>());//should work, position should be inherited, if not set the world position to RecipeForAssemblyObj world pos
        toyNamesForFinal = new string[RecipeObject.transform.childCount];//get children of gameobject, each child is a part of the assembly
        toyPartInFinalPos = new bool[RecipeObject.transform.childCount];

        for (int i = 0; i < RecipeObject.transform.childCount; i++)
        {
            toyNamesForFinal[i] = RecipeObject.transform.GetChild(i).name;
            toyPartInFinalPos[i] = false;
            GameObject Recent = Instantiate(emptyObj, new Vector3(0, 0, 0), Quaternion.identity, AssemblyPartsInPosition.GetComponent<Transform>());
            Recent.name = toyNamesForFinal[i] + " " + i.ToString();//setNameToSameAsToyNames + something to differentiate
        }
    }
    public void ChangeColliderTrigger(GameObject collObj, bool setToType)
    {
        Collider[] coll = collObj.GetComponents<Collider>();

        if (coll.Length == 1)//only a box collider
        {
            coll[0].isTrigger = setToType;
        }
        else//also another type
        {
            if (coll[0].GetType() == typeof(BoxCollider))//if is a box collider, make other not a trigger
            {
                coll[1].isTrigger = setToType;
            }
            else//2nd collider is a box collider, 1st is other type, !!!- means that there cannot be two box colliders
            {
                coll[0].isTrigger = setToType;
            }
        }
    }
}