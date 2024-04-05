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
    public GameObject mainRoomLocation;

    public bool[] consAdded;
    public DissolveController[] areas;// mainroom = 0, void = 1
    public List<DissolveController> updatingAreas;
    public footStepCon footStepCon;
    public Material boxMat;

    public float[] mainRoomDistances = new float[4];
    public bool firstFrameOutside = true;

    public float inverseSpeedOfDissappear;

    // Start is called before the first frame update
    void Start()
    {
        areas[1].updatingMats.Add(boxMat);
        consAdded = new bool[areas.Length];
        /*
        for (int i = 0; i < areas.Length; i++) // this makes it so that if an area is not the main room, it is invis by default
        {
            if (i != 1)
            {
                areas[i].setCutoffNoChange(new Vector3(0, 0, 0));

                for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                {
                    if (i != 0)//if not main room
                    {
                        areas[i].objsToEnable[j].SetActive(false);
                    }
                }
                SingleUpdateMat(i);

                areas[i].updatingMats.Clear();
            }
        }*/ // will want this active, just nice to be able to see each area for testing
    }

    // Update is called once per frame
    void Update()
    {
        AreaCheck();
        if (!inMainRoom())
        {
            WrapAround();
            if (firstFrameOutside)
            {
                areas[1].updatingMats.Add(boxMat);
                for (int i = 0; i < areas[0].updatingMats.Count; i++)
                {
                    if (areas[0].updatingMats[i] == boxMat)
                    {
                        areas[0].updatingMats.RemoveAt(i);
                    }
                }
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
            if (!firstFrameOutside)//(first frame inside)
            {
                removeSteps();
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
            footStepCon.setFootPrints(new Color(0, 255, 0), 1);//green
        }
    }
    public void removeSteps()
    {
        for (int i = 0; i < footStepCon.parentObj.transform.childCount; i++)
        {
            Destroy(footStepCon.parentObj.transform.GetChild(i).gameObject);
        }
        areas[0].updatingMats.Add(boxMat);
        for (int i = 0; i < areas[0].updatingMats.Count; i++)
        {
            if (areas[0].updatingMats[i] == boxMat)
            {
                areas[0].updatingMats.RemoveAt(i);
            }
        }
    }
    public void AreaCheck()//tells which area you are in
    {
        for (int i = 0; i < areas.Length; i++)
        {
            int inArea = 0;
            float distance = (Vector3.Distance(areas[i].centralPos, player.transform.position));
            if (distance < areas[i].size[1])//smaller area
            {
                inArea = 1;
                //Debug.Log(inArea + " area " + i);
            }
            else if (distance < areas[i].size[0])//bigger area
            {
                inArea = 2;
                //Debug.Log(inArea + " area " + i);
            }

            //Debug.Log(centerPos + " for area " + i);
            if (inArea == 1)//smaller area
            {
                if (consAdded[i])//cons added is asking if it is updating
                {
                    consAdded[i] = false;//its no longer updating
                    Debug.Log("Entered area " + i + ", section " + inArea);
                    areas[i].setCutoffNoChange(areas[i].startingDissolveDistances);

                    removeSteps();//remove footprints and then, if not entering main room, set footsteps back to main room
                    if (i != 0)
                    {
                        footStepCon.startingPointObj.transform.position = player.transform.position;
                        footStepCon.endingPointObj.transform.position = mainRoomLocation.transform.position;
                        footStepCon.setFootPrints(new Color(255, 255, 255), 0);
                    }

                    areas[i].transform.GetChild(0).position = areas[i].centralPos;//only needs to do once, sets what was following player to the center of its area
                    SingleUpdateMat(i);

                    areas[i].updatingMats.Clear();// actually no longer updating
                }
            }
            else if (inArea == 2)//bigger area
            {
                areas[i].transform.GetChild(0).position = player.transform.position;//sets the center to follow the player
                Vector3 cutoff = new Vector3(areas[i].startingDissolveDistances.x / ((distance - areas[i].size[1]) / inverseSpeedOfDissappear), areas[i].startingDissolveDistances.y, areas[i].startingDissolveDistances.z / ((distance - areas[i].size[1]) / inverseSpeedOfDissappear));
                areas[i].setCutoffNoChange(cutoff);

                if (!consAdded[i])//cons added is asking if it is updating, if its not updating, make it update
                {
                    consAdded[i] = true;
                    Debug.Log("Entered area " + i + ", section " + inArea);

                    for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                    {
                        areas[i].objsToEnable[j].SetActive(true);
                    }
                    for (int j = 0; j < areas[i].areaMats.Length; j++)
                    {
                        areas[i].updatingMats.Add(areas[i].areaMats[j]);
                    }
                }
            }
            else//in void
            {
                if (consAdded[i])
                {
                    consAdded[i] = false;
                    Debug.Log("Entered area " + i + ", section " + inArea);
                    areas[i].setCutoffNoChange(new Vector3(0, 0, 0));

                    for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                    {
                        areas[i].objsToEnable[j].SetActive(false);
                    }
                    SingleUpdateMat(i);

                    areas[i].updatingMats.Clear();//stop updating, area should no longer be in view
                }
            }
        }
    }
    public void SingleUpdateMat(int areaI)//turns into a list and then updates, terminates, (some issue)
    {
        List<Material> areaMatsList = new List<Material>();
        for (int j = 0; j < areas[areaI].areaMats.Length; j++)
        {
            areaMatsList.Add(areas[areaI].areaMats[j]);
        }
        areas[areaI].SetObjPos(areaMatsList);
    }
}
