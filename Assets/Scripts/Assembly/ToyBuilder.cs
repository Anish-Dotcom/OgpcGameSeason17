using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject stationObjsContainer;
    public GameObject objectsBeingUsedParent;
    public GameObject trueParent;

    public bool inBuildMode;
    public bool tinkering;//moving an object

    public float speed;
    public float clampSmallDist;
    public float clampBigDist;
    public float rotationSpeed;
    public float rotSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        stationCamStartPos = stationCam.transform.position;
        stationCamStartRot = stationCam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuildMode)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
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

            if (Input.GetKey(KeyCode.Mouse0) && trueParent.transform.childCount > 0)//rotating the toy
            {
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
                {
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(1));
                    objectsBeingUsedParent.transform.GetChild(0).transform.rotation = Quaternion.identity;
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(0));
                    objectsBeingUsedParent.transform.GetChild(0).Rotate(0, Input.GetAxis("Mouse X"), 0 * Time.deltaTime * rotSpeed);
                }
                else
                {
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(1));
                    objectsBeingUsedParent.transform.GetChild(0).transform.rotation = Quaternion.identity;
                    trueParent.transform.SetParent(objectsBeingUsedParent.transform.GetChild(0));
                    objectsBeingUsedParent.transform.GetChild(0).Rotate(0, 0, Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse2))//rotating the camera
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    float horiInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                    //Debug.Log(stationCamRig.transform.rotation.y * 100 + horiInput);
                    stationCamRig.transform.Rotate(Vector3.up, horiInput, Space.World);
                }
            }
        }
        else if (lookingAtCheck.lookingAt[myInfoIndex] && GetComponent<AssemblyController>().RecipeForAssemblyObj.GetComponent<Transform>().childCount == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))//add the object command
            {
                AddToBuilder(heldObjContainer.transform.GetChild(0).gameObject);
                ObjectPickUp.equipped = false;
                ObjectPickUp.slotFull = false;
            }
        }
        else if (lookingAtCheck.lookingAt[2])
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
                    playerMove.isControllable = true;
                    playerCam.SetActive(true);
                    stationCam.SetActive(false);
                    stationCam.transform.position = stationCamStartPos;
                    stationCam.transform.rotation = stationCam.transform.rotation;
                    movingCam = false;
                    timeMovingCam = 0;
                    inBuildMode = false;
                }
            }
        }
    }
    public void AddToBuilder(GameObject objToAdd)
    {
        if (trueParent.transform.childCount > 0)
        {
            objectsInStation.Add(objToAdd);
        }
        else
        {
            objToAdd.transform.SetParent(trueParent.transform);
            objToAdd.transform.position = trueParent.transform.position;
        }

    }
    public void EnterBuildMode()
    {
        velocity = Vector3.zero;
        deriv = Quaternion.identity;
        playerMove.isControllable = false;
        playerCam.SetActive(false);
        stationCam.SetActive(true);
        raycastCon.ClosePopups(-1);

        stationCam.transform.rotation = playerCam.transform.rotation;
        stationCam.transform.position = playerCam.transform.position;
        movingTo = true;
        movingCam = true;
    }
    public void ExitBuildMode()
    {
        velocity = Vector3.zero;
        deriv = Quaternion.identity;
        movingTo = false;
        movingCam = true;
    }
}