using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDissolveCon : MonoBehaviour
{
    public float[] wrapAroundDistances = new float[4];
    public GameObject player;
    public GameObject heldObjContainer;//what obj is held
    public GameObject boxObjContainer;//what things are bought
    public GameObject sellObjLocation;
    public GameObject cribLocation;

    public DissolveController[] areas;
    public footStepCon footStepCon;

    public float[] mainRoomDistances = new float[4];
    public bool firstFrameOutside = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inMainRoom())
        {
            WrapAround();
            if (firstFrameOutside)
            {
                placeSteps();//if outside of room give player a guide on where to go to
                firstFrameOutside = false;
            }
        }
        else 
        {
            removeSteps();
        }
    }
    private void WrapAround()
    {
        float changePosX = 0;
        float changePosZ = 0;
        if (player.transform.position.x >= wrapAroundDistances[0])
        {
            changePosX = -2 * transform.position.x + 0.5f;
            Debug.Log("Wrap around x" + transform.position.x);
        }
        else if (player.transform.position.x <= wrapAroundDistances[1])
        {
            changePosX = -2 * transform.position.x - 0.5f;
            Debug.Log("Wrap around -x" + transform.position.x);
        }
        else if (player.transform.position.z <= wrapAroundDistances[2])
        {
            changePosZ = -2 * transform.position.z - 0.5f;
            Debug.Log("Wrap around -z" + transform.position.z);
        }
        else if (player.transform.position.z >= wrapAroundDistances[3])
        {
            changePosZ = -2 * transform.position.z + 0.5f;
            Debug.Log("Wrap around z" + transform.position.z);
        }
        Vector3 playPos = new Vector3(player.transform.position.x + changePosX, player.transform.position.y, player.transform.position.z + changePosZ);
        player.transform.position = playPos;
        //Debug.Log(transform.position.x + transform.position.z);
    }
    private bool inMainRoom()
    {
        bool inRoom = true;//in room unless outside any boundary
        if (player.transform.position.x >= mainRoomDistances[0])
        {
            inRoom = false;
        }
        else if (player.transform.position.x <= mainRoomDistances[1])
        {
            inRoom = false;
        }
        else if (player.transform.position.z <= mainRoomDistances[2])
        {
            inRoom = false;
        }
        else if (player.transform.position.z >= mainRoomDistances[3])
        {
            inRoom = false;
        }
        return inRoom;
    }
    public void placeSteps()
    {

        footStepCon.startingPointObj.transform.position = player.transform.position;
        if (heldObjContainer.transform.childCount > 0)
        {
            if (heldObjContainer.transform.GetChild(0).CompareTag("finished Toy"))//holding a finished toy -- place steps to sell location
            {
                footStepCon.endingPointObj.transform.position = sellObjLocation.transform.position;
                footStepCon.setFootPrints(new Color(255, 0, 0));//red
            }
            if (heldObjContainer.transform.GetChild(0).CompareTag("childs Toy"))//golding your childs toy -- place steps to crib
            {
                footStepCon.endingPointObj.transform.position = cribLocation.transform.position;
                footStepCon.setFootPrints(new Color(0, 0, 255));//blue
            }
        }
        if (boxObjContainer.transform.childCount > 0)//there is a box for you to get
        {
            footStepCon.endingPointObj.transform.position = boxObjContainer.transform.position;
            footStepCon.setFootPrints(new Color(255, 255, 255));//white
        }
    }
    public void removeSteps()
    {

    }
}
