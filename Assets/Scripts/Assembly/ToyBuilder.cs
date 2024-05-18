using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    bool moving;
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
    private bool completingToy = false;
    private Vector3 completedLocation;
    private Vector3 velo;
    private GameObject toyCompleted;

    private bool painting;
    public Color currentColor;
    public GameObject paintingMenu;
    public GameObject cursorObj;//the cursor yours changes to when you are aiming at the paint bucket
    public GameObject paintBucketPos;
    public int PaintBucketUses = 0;
    public int PaintBucketMaxUses = 5;

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
        if (gameObject.GetComponent<AssemblyController>().building)
        {
            lookingAtCheck.lookingAt[1] = false;
            lookingAtCheck.lookingAt[2] = false;
            menuCon.closePopup(raycastCon.popups[5]);
        }
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
            if (tinkering)//moving the item around the scene or painting items
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && waitTimeAfterLockChange > 0.5f && tinkering)
                {
                    LockInPosition();
                }
                else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    if (Input.GetKey(KeyCode.Mouse0) && tinkering)//rotate the item upon the perpendicular axis
                    {
                        tinkeringObj.SetActive(true);
                        tinkeringObj = heldStationObjHolder.transform.GetChild(1).gameObject;
                        float angle2 = Input.GetAxis("Mouse X") * anglerSpeed;
                        tinkeringObj.transform.RotateAround(attachPointPos, -negitiveNorm, angle2);
                    }
                    else//painting
                    {
                        MoveTinkeringObj(true);
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
                    if (hit.transform.gameObject.name != "painbucket")
                    {
                        UnlockPosition(hit.transform.gameObject);
                    }
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
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                MoveTinkeringObj(false);
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
        if (moving)
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
                    movingCam = true;
                    moving = false;
                    timeMovingCam = 0;
                    inBuildMode = true;
                }
            }
            else//exiting build mode
            {
                if (timeMovingCam < 0.7)
                {
                    if (completingToy)
                    {
                        toyCompleted.transform.position = Vector3.SmoothDamp(toyCompleted.transform.position, completedLocation, ref velo, 0.3f);
                    }
                    if (movingCam)
                    {
                        stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, playerCam.transform.position, ref velocity, 0.3f);
                        stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, playerCam.transform.rotation, ref deriv, 0.3f);
                    }
                }
                else
                {
                    if (completingToy)
                    {
                        toyCompleted.transform.position = Vector3.SmoothDamp(toyCompleted.transform.position, completedLocation, ref velo, 0.1f);
                    }
                    if (movingCam)
                    {
                        stationCam.transform.position = Vector3.SmoothDamp(stationCam.transform.position, playerCam.transform.position, ref velocity, 0.1f);
                        stationCam.transform.rotation = QuaternionUtils.SmoothDamp(stationCam.transform.rotation, playerCam.transform.rotation, ref deriv, 0.1f);
                    }
                }
                if (timeMovingCam > 1)
                {
                    if (movingCam)
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
                    if (completingToy)
                    {
                        playerMove.gameObject.GetComponent<ObjectPickUp>().PickUp(toyCompleted);
                        completingToy = false;
                    }
                    moving = false;
                }
            }
        }
    }
    public void AddToBuilder(GameObject objToAdd)
    {
        if (objToAdd.name == "paintbucket")//adding the paint bucket to the station
        {
            if (paintBucketPos.transform.childCount == 0)
            {
                objToAdd.layer = 3;
                objToAdd.transform.position = paintBucketPos.transform.position;
                objToAdd.transform.rotation = Quaternion.identity;
                objToAdd.transform.SetParent(paintBucketPos.transform);
                objToAdd.transform.Rotate(-90, 0, 0);
                SwitchIfColliderEnabled(objToAdd, false);
            }
            else
            {
                Destroy(objToAdd);
            }
            PaintBucketUses += 5;
        }
        else if (trueParent.transform.childCount > 0 && objToAdd.name != "gearboxputtogether" && objToAdd.CompareTag("finished Toy") || objToAdd.name != "gearboxputtogether" && !objToAdd.CompareTag("finished Toy"))//any other parts
        {
            objectsInStation.Add(objToAdd);
            objToAdd.transform.SetParent(disabledStationObjsHolder.transform);
            objToAdd.SetActive(false);
            buildModeUi.addButtons(objToAdd);
        }
        else if (objToAdd.name == "gearboxputtogether")//central obj
        {
            objToAdd.transform.SetParent(trueParent.transform);
            objToAdd.transform.position = trueParent.transform.position;
            objToAdd.layer = 3;
            foreach (Transform child in objToAdd.transform)
            {
                child.gameObject.layer = 3;
            }
        }
        else if (objToAdd.CompareTag("finished Toy"))//re-adding a completed toy
        {
            objToAdd.transform.position = trueParent.transform.position;
            for (int i = 0; i <= objToAdd.transform.childCount; i++)
            {
                GameObject child = objToAdd.transform.GetChild(0).gameObject;
                SwitchIfColliderEnabled(child, true);
                child.layer = 3;//stationLayer
                foreach (Transform chilled in child.transform)
                {
                    chilled.gameObject.layer = 3;
                }
                child.transform.SetParent(trueParent.transform);
            }
            Destroy(objToAdd);
        }
    }
    public void EnterBuildMode()
    {
        completedLocation = heldObjContainer.transform.position;
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
        moving = true;
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
        moving = true;
        movingCam = true;
    }
    public void LockInPosition()
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
        buildModeUi.itemsAdded.RemoveAt(indexer);
        buildModeUi.UpdateButtons();
        buildModeUi.prevIndex = -2;
        LockInObj.transform.SetParent(trueParent.transform);
        tinkering = false;
    }
    public void UnlockPosition(GameObject unlockObj)
    {
        if (unlockObj.name != "gearboxputtogether" && !unlockObj.name.Contains("paintbucket"))
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
            tinkeringObj = unlockObj;
            tinkering = true;
        }
    }
    public void CompleteToy()
    {
        if (inBuildMode)
        {
            ExitBuildMode();
        }
        else
        {
            moving = true;
            movingTo = false;
        }
        completingToy = true;
        GameObject completedToy = Instantiate(gameObject.GetComponent<AssemblyController>().emptyObj, playerMove.gameObject.GetComponent<ObjectPickUp>().droppedObjectsContainer);
        completedToy.layer = 6;
        completedToy.name = "finished Toy";
        completedToy.tag = "finished Toy";
        completedToy.AddComponent(typeof(Rigidbody));
        //completedToy.GetComponent<Rigidbody>().useGravity = false;
        completedToy.GetComponent<Rigidbody>().isKinematic = true;
        completedToy.AddComponent(typeof(BoxCollider));//need to resize
        BoxCollider boxCol = completedToy.GetComponent<BoxCollider>();
        boxCol.isTrigger = true;
        completedToy.transform.position = trueParent.transform.position;
        for (int i = 0; i <= trueParent.transform.childCount; i++)
        {
            GameObject child = trueParent.transform.GetChild(0).gameObject;
            SwitchIfColliderEnabled(child, false);//disables collider used to pickup obj
            foreach (Transform chilled in child.transform)
            {
                chilled.gameObject.layer = 0;//defualt layer
            }
            child.transform.SetParent(completedToy.transform);
            child.layer = 0;//default
        }
        toyCompleted = completedToy;
    }
    public void MoveTinkeringObj(bool letMove)
    {
        RaycastHit hit;
        Ray ray = stationCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        LayerMask layer = LayerMask.GetMask("ToonLayer");//make work with raycast system
        if (Physics.Raycast(ray, out hit, 3.5f, layer))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.gameObject.name != "paintbucket")
            {
                if (tinkering)
                {
                    if (letMove)
                    {
                        cursorObj.SetActive(false);
                        negitiveNorm = -hit.normal;
                        tinkeringObj.SetActive(true);
                        tinkeringObj = heldStationObjHolder.transform.GetChild(1).gameObject;
                        if (hit.transform.gameObject != tinkeringObj)
                        {
                            heldStationObjHolder.transform.GetChild(0).position = hit.point;//just sets an empty object to the location of hit

                            //Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);
                            Debug.Log(tinkeringObj.name);
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
                }
                else if (painting)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        PaintPart(currentColor, hit.transform.gameObject);
                    }
                }
            }
            else//aiming at paintBucket
            {
                if (!painting)
                {
                    cursorObj.SetActive(true);
                    
                    if (Input.GetKeyDown(KeyCode.Mouse0))//open up painting menu
                    {
                        if (tinkering)
                        {
                            tinkering = false;
                            heldStationObjHolder.transform.GetChild(1).SetParent(disabledStationObjsHolder.transform);
                            tinkeringObj.SetActive(false);
                        }
                        painting = true;
                        currentColor = Color.white;
                        paintingMenu.SetActive(true);

                    }
                }
            }
        }
        else
        {
            if (!painting)
            {
                cursorObj.SetActive(false);
            }
        }
    }
    public void SwitchIfColliderEnabled(GameObject collObj, bool enabled)//remove box collider used for pickup, and rigidbody
    {
        Collider[] coll = collObj.GetComponents<Collider>();
        Destroy(collObj.GetComponent<Rigidbody>());

        if (coll.Length == 0)
        {
            return;
        }
        else if (coll.Length == 1)//only a box collider
        {
            coll[0].enabled = enabled;
        }
        else//also another type
        {
            if (coll[0].GetType() == typeof(BoxCollider))//if is a box collider, make other not a trigger
            {
                coll[0].enabled = enabled;
                coll[1].isTrigger = enabled;
            }
            else//2nd collider is a box collider, 1st is other type, !!!- means that there cannot be two box colliders
            {
                coll[1].enabled = enabled;
                coll[0].isTrigger = enabled;
            }
        }
    }
    public void PaintPart(Color color, GameObject hit)
    {
        if (!tinkering && inBuildMode)
        {
            if (hit.GetComponent<Collider>().gameObject.name != "painBucket")
            {
                hit.transform.GetComponent<Material>().SetColor("_BaseColor", color);
            }
        }
    }
    public void AvgColors(Color color1, Color color2)
    {
        color1 *= color1;
        color2 *= color2;

        Color resultingColor = color1 + color2;
        resultingColor *= (1 / 2);
        resultingColor.r = Mathf.Sqrt(resultingColor.r);
        resultingColor.g = Mathf.Sqrt(resultingColor.g);
        resultingColor.b = Mathf.Sqrt(resultingColor.b);

        currentColor = resultingColor;
        cursorObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = currentColor;
    }
    public void AddRed()
    {
        AvgColors(currentColor, Color.red);
    }
    public void AddBlue()
    {
        AvgColors(currentColor, Color.blue);
    }
    public void AddYellow()
    {
        AvgColors(currentColor, Color.yellow);
    }
}