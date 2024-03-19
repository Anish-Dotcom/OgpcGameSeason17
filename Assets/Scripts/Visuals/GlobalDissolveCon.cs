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

    public bool[] consAdded;
    public DissolveController[] areas;// mainroom = 0, void = 1
    public List<DissolveController> updatingAreas;
    public footStepCon footStepCon;

    public float[] mainRoomDistances = new float[4];
    public bool firstFrameOutside = true;

    // Start is called before the first frame update
    void Start()
    {
        consAdded = new bool[areas.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!inMainRoom())
        {
            WrapAround();
            AreaCheck();
            if (firstFrameOutside)
            {
                placeSteps();//if outside of room give player a guide on where to go to
                for (int i = 0; i < footStepCon.footPrintMatsInScene.Count; i++)
                {
                    areas[1].updatingMats.Add(footStepCon.footPrintMatsInScene[i]);
                }
                firstFrameOutside = false;
            }
        }
        else
        {
            if (!firstFrameOutside)
            {
                removeSteps();
                areas[1].updatingMats.Clear();
                firstFrameOutside = true;
            }
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
        //Debug.Log("In room " + inRoom);
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
                footStepCon.setFootPrints(new Color(255, 0, 0), 5);//red
            }
            if (heldObjContainer.transform.GetChild(0).CompareTag("childs Toy"))//golding your childs toy -- place steps to crib
            {
                footStepCon.endingPointObj.transform.position = cribLocation.transform.position;
                footStepCon.setFootPrints(new Color(0, 0, 255), 6);//blue
            }
        }
        if (boxObjContainer.transform.childCount > 0)//there is a box for you to get
        {
            footStepCon.endingPointObj.transform.position = boxObjContainer.transform.position;
            footStepCon.setFootPrints(new Color(255, 255, 255), 1);//white
        }
    }
    public void removeSteps()
    {
        for (int i = 0; i < footStepCon.parentObj.transform.childCount; i++)
        {
            Destroy(footStepCon.parentObj.transform.GetChild(i).gameObject);
        }
    }
    public void AreaCheck()//tells which area you are in
    {
        for (int i = 0; i < areas.Length; i++)
        {
            bool inArea = true;
            //in room unless outside any boundary
            int outsideX = 0;
            int outsideZ = 0;
            if (player.transform.position.x >= areas[i].size[0] + areas[i].gameObject.transform.GetChild(0).position.x)
            {
                inArea = false;
                outsideX = 1;
            }
            else if (player.transform.position.x <= areas[i].size[1] + areas[i].gameObject.transform.GetChild(0).position.x)//each dissolve con has a boundary
            {
                inArea = false;
                outsideX = -1;
            }
            else
            {
                outsideX = 0;
            }
            if (player.transform.position.z <= areas[i].size[2] + areas[i].gameObject.transform.GetChild(0).position.z)//child 0 is the center of the area
            {
                inArea = false;
                outsideZ = -1;
            }
            else if (player.transform.position.z >= areas[i].size[3] + areas[i].gameObject.transform.GetChild(0).position.z)
            {
                inArea = false;
                outsideZ = 1;
            }
            else
            {
                outsideZ = 0;
            }

            if (!inArea)
            {
                Vector3 centerPos = new Vector3(player.transform.position.x - areas[i].size[0] * outsideX, 0, player.transform.position.z - areas[i].size[3] * outsideZ);
                Debug.Log(centerPos);
                areas[i].transform.GetChild(1).position = centerPos;
                areas[i].centralObj = areas[i].transform.GetChild(1).gameObject;
                if (!consAdded[i])
                {
                    consAdded[i] = true;
                    for (int j = 0; j < areas[i].areaMats.Length; j++)
                    {
                        areas[i].updatingMats.Add(areas[i].areaMats[j]);
                    }
                }
            }
            else
            {

            }
            //not outside any of the boudaries, therefor in that area, return area num
        }
        //not in any area (void)
    }
}
