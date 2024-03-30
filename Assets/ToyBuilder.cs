using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBuilder : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject heldObjContainer;

    public GameObject stationCam;//cam you change to when building

    public GameObject[] objectsInStation;
    public GameObject stationObjsContainer;

    public bool inBuildMode;
    public bool tinkering;//moving an object

    public int rotSpeed = 12;
    public float friction = 0.5f;
    public float lerpSpeed = 1.5f;
    float xDeg;
    float yDeg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (lookingAtCheck.lookingAt[myInfoIndex])
        {
            if (Input.GetKeyDown(KeyCode.E))//add the object command
            {
                AddToBuilder(heldObjContainer.transform.GetChild(0).gameObject);
                ObjectPickUp.equipped = false;
                ObjectPickUp.slotFull = false;
            }
            else if (Input.GetKeyDown(KeyCode.F))//enter build mode
            {
                EnterBuildMode();
            }
        }
        else
        {

        }
    }
    public void AddToBuilder(GameObject objToAdd)
    {

    }
    public void EnterBuildMode()
    {
        //do camera movement and such
        inBuildMode = true;
    }
    public void RotateObj()//allows you to rotate an object using the movement of your mouse
    {
        Quaternion fromRotation;
        Quaternion toRotation;

        xDeg -= Input.GetAxis("Mouse X") * rotSpeed * friction;
        yDeg += Input.GetAxis("Mouse Y") * rotSpeed * friction;
        //fromRotation = transform.rotation;
        toRotation = Quaternion.Euler(yDeg, xDeg, 0);
        //transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
        Debug.Log("== " + toRotation);
    }
}
