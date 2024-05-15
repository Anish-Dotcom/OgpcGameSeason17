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
    public GameObject[] areaObjects;
    public float[] radiusPlacedUpon;
    public int inArea;
    public int trueArea;
    public List<DissolveController> updatingAreas;
    public footStepCon footStepCon;
    public List<Material> boxMats;

    public Material planeMat;

    public float[] mainRoomDistances = new float[4];
    public bool firstFrameOutside = true;

    public float inverseSpeedOfDissappear;
    private Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxMats.Count; i++)
        {
            areas[0].BoxSet(boxMats[i]);
            areas[0].BoxUpdate(boxMats[i]);
        }
        consAdded = new bool[areas.Length];
        /*
        for (int i = 0; i < areas.Length; i++) // this makes it so that if an area is not the main room, it is invis by default
        {
            if (i > 1)
            {
                areas[i].setCutoffNoChange(new Vector3(0, 0, 0));
                areas[i].setSecondDissolveRadius(0);

                for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                {
                    areas[i].objsToEnable[j].SetActive(false);
                }
                SingleUpdateMat(i);

                areas[i].updatingMats.Clear();
                areas[i].dualDissolveUpdatingMats.Clear();
            }
        }//*/ // will want this active, just nice to be able to see each area for testing
    }

    // Update is called once per frame
    void Update()
    {
        planeMat.SetVector("_Input_Pos", player.transform.position);
        AreaCheck();
        if (!inMainRoom())
        {
            WrapAround();
            for (int i = 0; i < boxMats.Count; i++)
            {
                areas[1].BoxUpdate(boxMats[i]);
                //Debug.Log(i + " area: 1");
            }
            if (firstFrameOutside)
            {
                SetRoomLocations();
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
                for (int i = 0; i < boxMats.Count; i++)
                {
                    areas[0].BoxUpdate(boxMats[i]);
                    //Debug.Log(i + " area: 0");
                }
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
    }
    public void AreaCheck()//tells which area you are in
    {
        for (int i = 0; i < areas.Length; i++)
        {
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
            else
            {
                inArea = -1;
            }

            //Debug.Log(centerPos + " for area " + i);
            if (inArea == 1)//smaller area
            {
                if (consAdded[i])//cons added is asking if it is updating
                {
                    consAdded[i] = false;//its no longer updating
                    decideArea(inArea, i);
                    //Debug.Log("Entered area " + i + ", section " + inArea);
                    //areas[i].setCutoffNoChange(areas[i].startingDissolveDistances);

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
                    areas[i].dualDissolveUpdatingMats.Clear();
                }
            }
            else if (inArea == 2)//bigger area
            {
                areas[i].transform.GetChild(0).position = player.transform.position;//sets the center to follow the player
                float cutDivide = ((distance - areas[i].size[1]) / inverseSpeedOfDissappear);
                //Debug.Log(cutDivide.ToString());
                if (cutDivide < 1)
                {
                    areas[i].setCutoffNoChange(areas[i].startingDissolveDistances);
                }
                else
                {
                    Vector3 cutoff = new Vector3(areas[i].startingDissolveDistances.x / (cutDivide), areas[i].startingDissolveDistances.y, areas[i].startingDissolveDistances.z / (cutDivide));
                    areas[i].setCutoffNoChange(cutoff);
                }
                float cut = areas[i].startingRadius / ((distance - areas[i].size[1]) / inverseSpeedOfDissappear);
                if (cut > areas[i].startingRadius) 
                {
                    areas[i].setSecondDissolveRadius(areas[i].startingRadius);
                }
                else
                {
                    areas[i].setSecondDissolveRadius(cut);
                }

                if (!consAdded[i])//cons added is asking if it is updating, if its not updating, make it update
                {
                    consAdded[i] = true;
                    decideArea(inArea, i);
                    //Debug.Log("Entered area " + i + ", section " + inArea);

                    for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                    {
                        areas[i].objsToEnable[j].SetActive(true);
                    }
                    for (int j = 0; j < areas[i].areaMats.Count; j++)
                    {
                        areas[i].updatingMats.Add(areas[i].areaMats[j]);
                    }
                    for (int j = 0; j < areas[i].dualDissolveAreaMats.Count; j++)
                    {
                        areas[i].dualDissolveUpdatingMats.Add(areas[i].dualDissolveAreaMats[j]);
                    }
                }
            }
            else//in void
            {
                if (consAdded[i])
                {
                    decideArea(inArea, i);
                    consAdded[i] = false;
                    //Debug.Log("Entered area " + i + ", section " + inArea);
                    areas[i].setCutoffNoChange(new Vector3(0, 0, 0));

                    for (int j = 0; j < areas[i].objsToEnable.Length; j++)
                    {
                        areas[i].objsToEnable[j].SetActive(false);
                    }
                    SingleUpdateMat(i);

                    areas[i].updatingMats.Clear();//stop updating, area should no longer be in view
                    areas[i].dualDissolveUpdatingMats.Clear();
                }
            }
        }
    }
    public void SingleUpdateMat(int areaI)//turns into a list and then updates, terminates, (some issue)
    {
        List<Material> areaMatsList = new List<Material>();
        for (int j = 0; j < areas[areaI].dualDissolveAreaMats.Count; j++)
        {
            areaMatsList.Add(areas[areaI].dualDissolveAreaMats[j]);
        }
        areas[areaI].setSecondDissolveCenter(areaMatsList);
        for (int j = 0; j < areas[areaI].areaMats.Count; j++)
        {
            areaMatsList.Add(areas[areaI].areaMats[j]);
        }
        areas[areaI].SetObjPos(areaMatsList);
    }
    public void decideArea(int InArea, int i)
    {
        if (InArea >= 0)
        {
            trueArea = i;
        }
        else
        {
            trueArea = -1;
        }
    }
    public void SetRoomLocations()
    {
        float[] angle = new float[areaObjects.Length];
        angle[0] = Random.Range(0f, 1f);
        float oneMinusAng = 1 - angle[0];
        Vector2 eitherSideNum = Vector2.zero;
        for (int i = 1; i < areaObjects.Length; i++)
        {
            if (oneMinusAng / i > angle[0] / (areaObjects.Length - i))// if the other side angle is greater than each divided up angle add one to other side, remove one from that side
            {
                eitherSideNum = new Vector2(i, areaObjects.Length - i);//x+y always adds to areaObjects.Length
            }
            else//in optimal angles
            {
                eitherSideNum = new Vector2(i - 1, areaObjects.Length - (i - 1));
                break;
            }
        }
        Vector2 eitherSideUsedNum = Vector2.one;
        for (int i = 0; i < areaObjects.Length; i++)
        {
            if (i > 0)
            {
                int side;
                //Debug.Log(eitherSideNum);
                if (eitherSideNum.x - (eitherSideUsedNum.x - 1) != 0 && eitherSideNum.y - (eitherSideUsedNum.y - 1) != 0)
                {
                    side = Random.Range(1, 3);
                }
                else if (eitherSideNum.x - (eitherSideUsedNum.x - 1) != 0)
                {
                    side = 1;
                }
                else
                {
                    side = 0;
                }
                //Debug.Log(side.ToString());
                if (side == 1)//left side
                {
                    angle[i] = angle[0] + (oneMinusAng / eitherSideNum.x) * eitherSideUsedNum.x;
                    //Debug.Log(angle[0].ToString() + " eitherSide: " + eitherSideNum.x.ToString() + " eitherSideUsed: " + eitherSideUsedNum.x.ToString());
                    eitherSideUsedNum += new Vector2(1, 0);
                }
                else//right side
                {
                    angle[i] = -1 * angle[0] - (angle[0] / eitherSideNum.y) * eitherSideUsedNum.y;
                    //Debug.Log(angle[0].ToString() + " eitherSide: " + eitherSideNum.x.ToString() + " eitherSideUsed: " + eitherSideUsedNum.x.ToString());
                    eitherSideUsedNum += new Vector2(0, 1);
                }
            }
            //Debug.Log("angle " + angle[i] + " sinangle: " + Mathf.Cos(angle[i]).ToString() + " radius: " + radiusPlacedUpon[i]);
            Debug.Log("angle " + angle[i] + " area " + i);
            areaObjects[i].transform.position = new Vector3(Mathf.Cos(angle[i]) * radiusPlacedUpon[i], transform.position.y, Mathf.Sin(angle[i]) * radiusPlacedUpon[i]);

            Vector3 direction = areaObjects[i].transform.GetChild(0).GetChild(0).position - areaObjects[i].transform.GetChild(0).position;
            float ang = Vector3.Angle(direction, areas[0].centralPos - areaObjects[i].transform.position);
            Vector3 axis1 = Vector3.Cross(direction, areas[0].centralPos - areaObjects[i].transform.position).normalized;
            if (axis1 != Vector3.zero)
            {
                axis = axis1;
            }
            //Debug.Log("axis: " + axis);
            // rotate around the attachPointPos
            areaObjects[i].transform.RotateAround(areaObjects[i].GetComponent<DissolveController>().centralObj.transform.position, axis, ang);

            List<Material> areaMatsT = new List<Material>();
            //List<Material> dualAreaMats = new List<Material>();
            for (int j = 0; j < areaObjects[i].GetComponent<DissolveController>().dualDissolveAreaMats.Count; j++)//add just the mats with dual
            {
                areaMatsT.Add(areaObjects[i].GetComponent<DissolveController>().dualDissolveAreaMats[j]);
            }
            areaObjects[i].GetComponent<DissolveController>().objNonMoveCenterSet(areaMatsT);//set center for dual

            for (int j = 0; j < areaObjects[i].GetComponent<DissolveController>().areaMats.Count; j++)//add the other mats
            {
                areaMatsT.Add(areaObjects[i].GetComponent<DissolveController>().areaMats[j]);
            }
            areaObjects[i].GetComponent<DissolveController>().SetObjPos(areaMatsT);//set other center for both
        }
    }
}
