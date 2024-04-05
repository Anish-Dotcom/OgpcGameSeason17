using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCon : MonoBehaviour
{
    public GameObject[] colliderObjs;//colliders raycast can hit with
    public GameObject[] colliderNonheldObjs;//colliders raycast can hit that dont require a held obj check
    public GameObject[] popups;//all pop ups
    public LayerMask pickupMask;

    public GameObject playerCam;
    public MenuController menuController;

    public GameObject heldObjContainer;

    public string lookingAt;

    public int lastK;
    public int lastI;
    public int lastType;

    public bool returned;
    void Start()
    {
        returned = false;
        lastI = -1;
        lastK = -1;
        lastType = -1;
        lookingAt = "false";
    }

    // Update is called once per frame
    void Update()
    {
        returned = false;

        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//hitting something
        {
            //Debug.Log(hit.transform.gameObject.layer.ToString());
            RaycastHit hit1;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit1, 3.5f, pickupMask))
            {
                PickupObjectCheck(hit1);
                if (returned)
                {
                    return;
                }
            }
            else if (heldObjContainer.transform.childCount > 0)//holding an object
            {
                colliderObjsCheck(hit);
                if (returned)
                {
                    return;
                }

                colliderNonHeldObjsCheck(hit);//hitting something, just not one of the colliderObjs, so check other
                if (returned)
                {
                    return;
                }

                //colliderNonheldObjs check
                ClosePopups(-1);
                DisableLastLookingAt();
                lastI = -1;
                lastK = -1;
                lastType = -1;
            }
            else
            {
                colliderNonHeldObjsCheck(hit);
                if (returned)
                {
                    return;
                }
                //hitting something, just not one of the colliderNonheldObjs
                ClosePopups(-1);
                DisableLastLookingAt();
                lastI = -1;
                lastK = -1;
                lastType = -1;
            }
        }
        else
        {
            ClosePopups(-1);
            DisableLastLookingAt();
            lastI = -1;
            lastK = -1;
            lastType = -1;
        }
    }
    public void DisableLastLookingAt()
    {
        if (lastType == 0)
        {
            if (lastI != -1 || lastK != -1)
            {
                colliderObjs[lastI].GetComponent<PopupInfo>().lookingAt[lastK] = false;
            }
        }
        else if (lastType == 1)
        {
            if (lastI != -1 || lastK != -1)
            {
                colliderNonheldObjs[lastI].GetComponent<PopupInfo>().lookingAt[lastK] = false;
            }
        }
        else if (lastType == 2)
        {
            if (lastK != -1)
            {
                GetComponent<PopupInfo>().lookingAt[lastK] = false;
            }
        }
    }
    public void ClosePopups(int minus)
    {
        if (minus > -1)
        {
            for (int j = 0; j < popups.Length; j++)
            {
                if (j != minus)
                {
                    menuController.closePopup(popups[j]);//set all popups to inactive
                }
            }
        }
        else
        {
            for (int j = 0; j < popups.Length; j++)
            {
                menuController.closePopup(popups[j]);
            }
        }
    }
    public void colliderObjsCheck(RaycastHit hit)
    {
        for (int i = 0; i < colliderObjs.Length; i++)
        {
            if (hit.transform.gameObject == colliderObjs[i])
            {
                for (int k = 0; k < colliderObjs[i].GetComponent<PopupInfo>().heldObjTag.Length; k++)
                {
                    string currentTag = colliderObjs[i].GetComponent<PopupInfo>().heldObjTag[k];
                    if (currentTag != "")
                    {
                        if (heldObjContainer.transform.GetChild(0).CompareTag(colliderObjs[i].GetComponent<PopupInfo>().heldObjTag[k]))
                        {
                            if (lastK != k || lastI != i)//the thing being looked at changed
                            {
                                lookingAt = "looking at " + colliderObjs[i].name;
                                int popupIndex = colliderObjs[i].GetComponent<PopupInfo>().popupIndex[k];
                                menuController.openPopup(popups[popupIndex]);
                                colliderObjs[i].GetComponent<PopupInfo>().lookingAt[k] = true;

                                ClosePopups(popupIndex);

                                DisableLastLookingAt();
                                lastI = i;
                                lastK = k;
                            }
                            returned = true;
                            return;
                        }
                    }
                }//will get here if looking at something but not holding the right obj
                ClosePopups(-1);
                DisableLastLookingAt();
                lastI = i;
                lastK = -1;
                lastType = -1;
                returned = true;
                return;
            }
        }
    }
    public void colliderNonHeldObjsCheck(RaycastHit hit)
    {
        for (int i = 0; i < colliderNonheldObjs.Length; i++)
        {
            if (hit.transform.gameObject == colliderNonheldObjs[i])
            {
                for (int k = 0; k < colliderNonheldObjs[i].GetComponent<PopupInfo>().heldObjTag.Length; k++)
                {
                    if (colliderNonheldObjs[i].GetComponent<PopupInfo>().heldObjTag[k] == "")//doesnt require held obj
                    {
                        if (lastK != k || lastI != i)
                        {
                            if (i == 0)
                            {
                                if (colliderNonheldObjs[i].GetComponent<AssemblyController>().RecipeForAssemblyObj.GetComponent<Transform>().childCount > 0)
                                {
                                    ClosePopups(-1);
                                    DisableLastLookingAt();
                                    lastI = i;
                                    lastK = k;
                                    lastType = 1;
                                    returned = true;
                                    return;
                                }
                                else
                                {
                                    lookingAt = "looking at " + colliderNonheldObjs[i].name;
                                    int popupIndex = colliderNonheldObjs[i].GetComponent<PopupInfo>().popupIndex[k];
                                    menuController.openPopup(popups[popupIndex]);
                                    colliderNonheldObjs[i].GetComponent<PopupInfo>().lookingAt[k] = true;

                                    ClosePopups(popupIndex);

                                    DisableLastLookingAt();
                                    lastI = i;
                                    lastK = k;
                                    lastType = 1;
                                }
                            }
                            else
                            {
                                lookingAt = "looking at " + colliderNonheldObjs[i].name;
                                int popupIndex = colliderNonheldObjs[i].GetComponent<PopupInfo>().popupIndex[k];
                                menuController.openPopup(popups[popupIndex]);
                                colliderNonheldObjs[i].GetComponent<PopupInfo>().lookingAt[k] = true;

                                ClosePopups(popupIndex);

                                DisableLastLookingAt();
                                lastI = i;
                                lastK = k;
                                lastType = 1;
                            }
                        }
                        returned = true;
                        return;
                    }
                }
                ClosePopups(-1);
                DisableLastLookingAt();
                lastI = i;
                lastK = -1;
                lastType = -1;
                returned = true;
                return;
            }
        }
    }
    public void PickupObjectCheck(RaycastHit hit)
    {
        for (int k = 0; k < GetComponent<PopupInfo>().heldObjTag.Length; k++)
        {
            if (GetComponent<PopupInfo>().heldObjTag[k] == hit.transform.gameObject.layer.ToString())//doesnt require held obj
            {
                if (k == 1)//storage
                {
                    if (heldObjContainer.transform.childCount > 0)
                    {
                        if (lastK != k || lastI != 99)
                        {
                            lookingAt = "looking at " + hit.transform.name;
                            int popupIndex = GetComponent<PopupInfo>().popupIndex[k];
                            menuController.openPopup(popups[popupIndex]);

                            GetComponent<PopupInfo>().lookingAt[k] = true;
                            GetComponent<PopupInfo>().hitObj[k] = hit.transform.gameObject;

                            ClosePopups(popupIndex);

                            DisableLastLookingAt();
                            lastI = 99;
                            lastK = k;
                            lastType = 2;
                        }
                        returned = true;
                        return;
                    }
                }
                else if (lastK != k || lastI != 100)
                {
                    lookingAt = "looking at " + hit.transform.name;
                    int popupIndex = GetComponent<PopupInfo>().popupIndex[k];
                    menuController.openPopup(popups[popupIndex]);

                    GetComponent<PopupInfo>().lookingAt[k] = true;
                    GetComponent<PopupInfo>().hitObj[k] = hit.transform.gameObject;

                    ClosePopups(popupIndex);

                    DisableLastLookingAt();
                    lastI = 100;
                    lastK = k;
                    lastType = 2;
                }
                returned = true;
                return;
            }
        }
        ClosePopups(-1);
        DisableLastLookingAt();
        lastI = -1;
        lastK = -1;
        lastType = 2;
        returned = true;
        return;
    }
}