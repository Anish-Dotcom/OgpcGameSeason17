using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToyBuilder : MonoBehaviour
{
    public RaycastCon raycastCon;
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject heldObjContainer;
    public PlayerMove playerMove;
    public GameObject playerCam;

    public GameObject stationCam;//cam you change to when building
    Vector3 stationCamStartPos;
    Quaternion stationCamStartRot;
    public GameObject stationCamRig;
    bool movingCam;
    bool movingTo;
    float timeMovingCam;
    Vector3 velocity;
    Quaternion deriv;

    public List<GameObject> objectsInStation;
    public GameObject disabledStationObjsHolder;
    public GameObject heldStationObjHolder;
    public GameObject stationObjsContainer;
    public GameObject objectsBeingUsedParent;
    public GameObject trueParent;

    public GameObject crosshairOutisde;
    public GameObject stationMenu;
    public MenuController menuCon;
    public BuildModeUI buildModeUi;
    public int indexer;

    public GameObject tinkeringObj;

    public bool inBuildMode;
    public bool tinkering;//moving an object

    public float speed;
    public float clampSmallDist;
    public float clampBigDist;
    public float rotationSpeed;
    public float rotSpeed = 50;

    private Quaternion resetRot;
    private float angle;
    public float anglerSpeed;
    private Vector3 axis;
    private Vector3 attachPointPos;
    private Vector3 referPos;
    private Vector3 negitiveNorm;

    private float waitTimeAfterLockChange;

    // Start is called before the first frame update
    void Start()
    {
        resetRot = Quaternion.identity;
        stationCamStartPos = stationCam.transform.position;
        stationCamStartRot = stationCam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        waitTimeAfterLockChange += Time.deltaTime;
        if (inBuildMode)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                CompleteToy();
            }
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                ExitBuildMode();
            }
            float distance = Vector3.Distance(stationCam.transform.position, stationCamRig.transform.position);
            if (Input.GetAxis("Mouse ScrollWheel") != 0)//zooming camera
            {
                if (distance > clampBigDist)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        stationCam.transform.position += Input.GetAxis("Mouse ScrollWheel") * stationCam.transform.forward * speed;
                    }
                }
                else if (distance < clampSmallDist)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        stationCam.transform.position += Input.GetAxis("Mouse ScrollWheel") * stationCam.transform.forward * speed;
                    }
                }
                else
                {
                    stationCam.transform.position += Input.GetAxis("Mouse ScrollWheel") * stationCam.transform.forward * speed;
                }
            }

            GameObject mainParent = objectsBeingUsedParent.transform.GetChild(0).gameObject;
            if (tinkering)//moving the item around the scene
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && waitTimeAfterLockChange > 0.5f)
                {
                    LockInPosition();
                }
                else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    if (Input.GetKey(KeyCode.Mouse0))//rotate the item upon the perpendicular axis
                    {
                        tinkeringObj.SetActive(true);
                        tinkeringObj = heldStationObjHolder.transform.GetChild(1).gameObject;
                        float angle2 = Input.GetAxis("Mouse X") * anglerSpeed;
                        tinkeringObj.transform.RotateAround(attachPointPos, -negitiveNorm, angle2);
                    }
                    else
                    {
                        MoveTinkeringObj();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && waitTimeAfterLockChange > 0.5f)//right click - unlock toy
            {
                RaycastHit hit;
                Ray ray = stationCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                LayerMask layer = LayerMask.GetMask("ToonLayer");//make work with raycast system
                if (Physics.Raycast(ray, out hit, 3.5f, layer))
                {
                    UnlockPosition(hit.transform.gameObject);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse0) && trueParent.transform.childCount > 0)//rotating the toy
            {
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
                {
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(1));
                    mainParent.transform.rotation = resetRot;
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(0));
                    mainParent.transform.Rotate(0, Input.GetAxis("Mouse X") * -1 * Time.deltaTime * rotSpeed, 0);
                }
                else
                {
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(1));
                    mainParent.transform.rotation = resetRot;
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(0));
                    mainParent.transform.Rotate(0, 0, Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse2))//rotating the camera
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    float horiInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                    stationCamRig.transform.Rotate(Vector3.up, horiInput, Space.World);

                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(1));
                    mainParent.transform.rotation = stationCamRig.transform.rotation;
                    resetRot = stationCamRig.transform.rotation;
                    trueParent.transform.SetParent(mainParent.transform);
                }
            }
        }
        else if (lookingAtCheck.lookingAt[myInfoIndex] && GetComponent<AssemblyController>().RecipeForAssemblyObj.GetComponent<Transform>().childCount == 0 || lookingAtCheck.lookingAt[2] && GetComponent<AssemblyController>().RecipeForAssemblyObj.GetComponent<Transform>().childCount == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))//add the object command
            {
                AddToBuilder(heldObjContainer.transform.GetChild(0).gameObject);
                ObjectPickUp.equipped = false;
                ObjectPickUp.slotFull = false;
                lookingAtCheck.lookingAt[myInfoIndex] = false;
            }
        }
        else if (lookingAtCheck.lookingAt[3])
        {
            if (Input.GetKeyDown(KeyCode.F))//enter build mode
            {
                EnterBuildMode();
            }
        }
        if (movingCam)
        {
            timeMovingCam += Time.deltaTime;
            if (movingTo)//(entering build mode)
            {
                if (timeMovingCam < 0.7)
                {
                    stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, stationCamStartPos, ref velocity, 0.3f);
                    stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, stationCamStartRot, ref deriv, 0.3f);
                }
                else
                {
                    stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, stationCamStartPos, ref velocity, 0.1f);
                    stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, stationCamStartRot, ref deriv, 0.1f);
                }
                if (timeMovingCam > 1)
                {
                    stationCam.transform.position = stationCamStartPos;
                    stationCam.transform.rotation = stationCam.transform.rotation;
                    movingCam = false;
                    timeMovingCam = 0;
                    inBuildMode = true;
                }
            }
            else//exiting build mode
            {
                if (timeMovingCam < 0.7)
                {
                    stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, playerCam.transform.position, ref velocity, 0.3f);
                    stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, playerCam.transform.rotation, ref deriv, 0.3f);
                }
                else
                {
                    stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, playerCam.transform.position, ref velocity, 0.1f);
                    stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, playerCam.transform.rotation, ref deriv, 0.1f);
                }
                if (timeMovingCam > 1)
                {
                    raycastCon.ReopenPopups();
                    playerMove.isControllable = true;
                    playerCam.SetActive(true);
                    stationCam.SetActive(false);
                    stationCam.transform.position = stationCamStartPos;
                    stationCam.transform.rotation = stationCam.transform.rotation;
                    movingCam = false;
                    timeMovingCam = 0;
                }
            }
        }
    }
    public void AddToBuilder(GameObject objToAdd)
    {
        if (trueParent.transform.childCount > 0 && !objToAdd.transform.CompareTag("Central Assembly") || !objToAdd.transform.CompareTag("Central Assembly"))//any other parts
        {
            objectsInStation.Add(objToAdd);
            objToAdd.transform.SetParent(disabledStationObjsHolder.transform);
            objToAdd.SetActive(false);
            buildModeUi.addButtons(objToAdd);
        }
        else//central obj
        {
            objToAdd.transform.SetParent(trueParent.transform);
            objToAdd.transform.position = trueParent.transform.position;
            objToAdd.layer = 3;
            foreach (Transform child in objToAdd.transform)
            {
                child.gameObject.layer = 3;
            }
        }
    }
    public void EnterBuildMode()
    {
        menuCon.openMenu(stationMenu, false, false, crosshairOutisde, true);
        velocity = Vector3.zero;
        deriv = Quaternion.identity;
        playerMove.isControllable = false;
        playerCam.SetActive(false);
        stationCam.SetActive(true);
        raycastCon.ClosePopups(-1, true);

        stationCam.transform.rotation = playerCam.transform.rotation;
        stationCam.transform.position = playerCam.transform.position;
        movingTo = true;
        movingCam = true;
    }
    public void ExitBuildMode()
    {
        if (tinkering)
        {
            tinkering = false;
            heldStationObjHolder.transform.GetChild(1).SetParent(disabledStationObjsHolder.transform);
            tinkeringObj.SetActive(false);
        }
        crosshairOutisde.SetActive(true);
        inBuildMode = false;
        menuCon.closeMenu(stationMenu);
        velocity = Vector3.zero;
        deriv = Quaternion.identity;
        movingTo = false;
        movingCam = true;
    }
    public void LockInPosition()//doesnt work
    {
        waitTimeAfterLockChange = 0;
        GameObject LockInObj = heldStationObjHolder.transform.GetChild(1).gameObject;
        LockInObj.layer = 3;
        foreach (Transform child in LockInObj.transform)
        {
            child.gameObject.layer = 3;
        }
        Destroy(buildModeUi.prefabButtons[indexer]); // removes it from the hotbar
        buildModeUi.prefabButtons.RemoveAt(indexer);
        LockInObj.transform.SetParent(trueParent.transform);
        tinkering = false;
    }
    public void UnlockPosition(GameObject unlockObj)//doesnt work
    {
        if (unlockObj != trueParent.transform.GetChild(0).gameObject)
        {
            unlockObj.layer = 0;
            foreach (Transform child in unlockObj.transform)
            {
                child.gameObject.layer = 0;
            }
            waitTimeAfterLockChange = 0;
            //re-add button
            buildModeUi.addButtons(unlockObj);//re-add button, does this work, idk?
            unlockObj.transform.SetParent(heldStationObjHolder.transform);
            tinkering = true;
        }
    }
    public void CompleteToy()
    {
        ExitBuildMode();
        GameObject completedToy = Instantiate(gameObject.GetComponent<AssemblyController>().emptyObj, playerMove.gameObject.GetComponent<ObjectPickUp>().droppedObjectsContainer);
        completedToy.layer = 6;
        completedToy.AddComponent(typeof(Rigidbody));
        completedToy.AddComponent(typeof(BoxCollider));//shouldnt need this
        BoxCollider boxCol = completedToy.GetComponent<BoxCollider>();
        boxCol.isTrigger = true;
        completedToy.transform.position = trueParent.transform.position;
        //completedToy.name = ToyBuilder.type;
        //give completed toy colliders
        for (int i = 0; i <= trueParent.transform.childCount; i++)
        {
            GameObject child = trueParent.transform.GetChild(0).gameObject;
            ScrapCollider(child);//gets rid of collider used to pickup obj
            foreach (Transform chilled in child.transform)
            {
                chilled.gameObject.layer = 6;
            }
            child.transform.SetParent(completedToy.transform);
            child.layer = 6;//pickupable
        }
    }
    public void MoveTinkeringObj()
    {
        RaycastHit hit;
        Ray ray = stationCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        LayerMask layer = LayerMask.GetMask("ToonLayer");//make work with raycast system
        if (Physics.Raycast(ray, out hit, 3.5f, layer))
        {
            negitiveNorm = -hit.normal;
            tinkeringObj.SetActive(true);
            tinkeringObj = heldStationObjHolder.transform.GetChild(1).gameObject;
            if (hit.transform.gameObject != tinkeringObj)
            {
                heldStationObjHolder.transform.GetChild(0).position = hit.point;//just sets an empty object to the location of hit

                //Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);
                attachPointPos = tinkeringObj.GetComponent<attachInfo>().attachPoint.transform.position;
                referPos = tinkeringObj.GetComponent<attachInfo>().refer.transform.position;

                //angle between current vectors
                angle = Vector3.Angle(referPos - attachPointPos, -hit.normal);
                // Calculate the axis of rotation
                Vector3 axis1 = Vector3.Cross(referPos - attachPointPos, -hit.normal).normalized;
                if (axis1 != Vector3.zero)
                {
                    axis = axis1;
                }
                //Debug.Log("axis: " + axis);
                // rotate around the attachPointPos
                tinkeringObj.transform.RotateAround(attachPointPos, axis, angle);

                //then compare position and move
                tinkeringObj.transform.position = hit.point - (tinkeringObj.GetComponent<attachInfo>().attachPoint.transform.position - tinkeringObj.transform.position);//sets the postition to where the attach point will be at the hit point, facing into the mesh
            }
        }
        else
        {
            tinkeringObj.SetActive(false);
        }
    }
    public void ScrapCollider(GameObject collObj)//remove box collider used for pickup, and rigidbody
    {
        Collider[] coll = collObj.GetComponents<Collider>();
        Destroy(collObj.GetComponent<Rigidbody>());

        if (coll.Length == 1)//only a box collider
        {
            Destroy(coll[0]);
        }
        else//also another type
        {
            if (coll[0].GetType() == typeof(BoxCollider))//if is a box collider, make other not a trigger
            {
                Destroy(coll[0]);
                coll[1].isTrigger = false;
            }
            else//2nd collider is a box collider, 1st is other type, !!!- means that there cannot be two box colliders
            {
                Destroy(coll[1]);
                coll[0].isTrigger = false;
            }
        }
    }
}