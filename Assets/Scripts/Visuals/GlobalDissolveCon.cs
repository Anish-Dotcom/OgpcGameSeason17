using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDissolveCon : MonoBehaviour
{
    public float[] wrapAroundDistances = new float[4];
    public GameObject player;
    public GameObject heldObjContainer;//what obj is held
    public GameObject boxObjContainer;//what things are bought

    public DissolveController[] areas;
    public footStepCon footStepCon;

    public float[] mainRoomDistances = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!inMainRoom())
        {
            placeSteps();//if outside of room give player a guide on where to go to
            WrapAround();
        }*/
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
        Vector3 playPos = new Vector3(transform.position.x + changePosX, transform.position.y, transform.position.z + changePosZ);
        transform.position = playPos;
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
        //if statements put further up are chosen first
        //other option is there are multiple footstep paths
        if (heldObjContainer.transform.GetChild(0).CompareTag("finished Toy"))
        {

        }
        else if (heldObjContainer.transform.GetChild(0).CompareTag("childs Toy"))
        {

        }
        else if (boxObjContainer.transform.childCount > 0)//there is a box for you to get
        {

        }
    }
}
