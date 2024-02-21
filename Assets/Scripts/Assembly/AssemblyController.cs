using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{

    public GameObject assembly;
    public GameObject finalObj;

    //above are for testing

    public Material recipeMat;

    public bool lookingAt = false;
    public float distFromPlayer;
    public GameObject playerCam;
    public GameObject heldObjContainer;
    public GameObject addPart;

    public MenuController menuController;

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

        //for testing:
        SetAssembly(assembly, finalObj);
    }
    void Update()
    {
        distFromPlayer = Vector3.Distance(playerCam.transform.position, transform.position);
        if (distFromPlayer <= 3.8 && RecipeForAssemblyObj.GetComponent<Transform>().childCount > 0)
        {
            RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
            recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer));
        }
        else if (distFromPlayer <= 3.9 && RecipeForAssemblyObj.GetComponent<Transform>().childCount > 0)
        {
            RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
            recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer * 1.5f));
        }
        else if (distFromPlayer <= 4 && RecipeForAssemblyObj.GetComponent<Transform>().childCount > 0)
        {
            RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(true);
            recipeMat.SetFloat("_Transparency", 0.5f / (distFromPlayer * distFromPlayer * distFromPlayer * 2));
        }
        else if (distFromPlayer > 4)
        {
            RecipeForAssemblyObj.transform.GetChild(0).gameObject.SetActive(false);
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//working
        {
            if (heldObjContainer.transform.childCount > 0 && RecipeForAssemblyObj.transform.childCount > 0 && hit.collider.gameObject.CompareTag("assemblyStation"))
            {
                if (heldObjContainer.transform.GetChild(0).CompareTag("assemblyPart"))
                {
                    lookingAt = true;
                    menuController.openPopup(addPart);
                }
                else
                {
                    lookingAt = false;
                    menuController.closePopup(addPart);
                }
            }
            else
            {
                lookingAt = false;
                menuController.closePopup(addPart);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && lookingAt)
        {
            AddToAssembly(heldObjContainer.GetComponent<Transform>().GetChild(0).gameObject);
            ObjectPickUp.equipped = false;
            ObjectPickUp.slotFull = false;
        }


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

    private void AddToAssembly(GameObject objToAdd)//not working yet, everything else in script should work
    {
        Debug.Log("addItemFunctionCalled" + timePassed);
        if (objToAdd.CompareTag("assemblyPart"))
        {
            for (int b = 0; b < toyNamesForFinal.Length; b++)
            {
                if (objToAdd.name == toyNamesForFinal[b] && toyPartInFinalPos[b] == false)//
                {
                    Collider[] coll = objToAdd.GetComponents<Collider>();

                    if (coll.Length == 1)//only a box collider
                    {
                        coll[0].isTrigger = false;
                    }
                    else//also another type
                    {
                        if (coll[0].GetType() == typeof(BoxCollider))//if is a box collider, make other not a trigger
                        {
                            coll[1].isTrigger = false;
                        }
                        else//2nd collider is a box collider, 1st is other type, !!!- means that there cannot be two box colliders
                        {
                            coll[0].isTrigger = false;
                        }
                    }

                    objToAdd.GetComponent<Rigidbody>().isKinematic = true;
                    Vector3 velocity = objToAdd.GetComponent<Rigidbody>().velocity;

                    RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(b).gameObject.SetActive(false);//disable outline same as obj

                    objToAdd.GetComponent<Transform>().SetParent(AssemblyPartsInPosition.GetComponent<Transform>().GetChild(b).GetComponent<Transform>());//sets to be a child of the assosciated
                    objToAdd.transform.rotation = RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(b).transform.rotation;
                    objToAdd.transform.position = RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(b).transform.position;
                    //objToAdd.transform.position = Vector3.SmoothDamp(objToAdd.transform.position, RecipeForAssemblyObj.GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(b).transform.position, ref velocity, 1);
                    toyPartInFinalPos[b] = true;

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

    //outline
    public void SetAssembly(GameObject RecipeObj, GameObject CompletedAssembly)
    {
        completedAssembly = CompletedAssembly;
        GameObject RecipeObject = Instantiate(RecipeObj, RecipeForAssemblyObj.transform.position + RecipeObj.transform.position, Quaternion.identity, RecipeForAssemblyObj.GetComponent<Transform>());//should work, position should be inherited, if not set the world position to RecipeForAssemblyObj world pos
        toyNamesForFinal = new string[RecipeObject.transform.childCount];//get children of gameobject, each child is a part of the assembly
        toyPartPreFinalPos = new Vector3[RecipeObject.transform.childCount];
        toyPartFinalPos = new Vector3[completedAssembly.transform.childCount];
        toyPartInFinalPos = new bool[RecipeObject.transform.childCount];

        for (int i = 0; i < RecipeObject.transform.childCount; i++)
        {
            toyNamesForFinal[i] = RecipeObject.transform.GetChild(i).name;
            toyPartPreFinalPos[i] = RecipeObject.transform.GetChild(i).transform.position;
            toyPartFinalPos[i] = CompletedAssembly.transform.GetChild(i).transform.position;
            toyPartInFinalPos[i] = false;
            GameObject Recent = Instantiate(emptyObj, new Vector3(0, 0, 0), Quaternion.identity, AssemblyPartsInPosition.GetComponent<Transform>());
            Recent.name = toyNamesForFinal[i] + " " + i.ToString();//setNameToSameAsToyNames + something to differentiate
        }
    }

    public void OnMouseExit()
    {
        lookingAt = false;
    }
}