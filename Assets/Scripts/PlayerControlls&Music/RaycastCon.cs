using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCon : MonoBehaviour
{
    public GameObject[] colliderObjs;//colliders raycast can hit with
    public GameObject[] colliderNonheldObjs;//colliders raycast can hit that dont require a held obj check
    public GameObject[] popups;//all pop ups

    public GameObject playerCam;
    public MenuController menuController;

    public GameObject heldObjContainer;

    public string lookingAt;
    void Start()
    {
        lookingAt = "false";
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//hitting something
        {
            if (heldObjContainer.transform.childCount > 0)//holding an object
            {
                for (int i = 0; i < colliderObjs.Length; i++)
                {
                    if (hit.transform.gameObject == colliderObjs[i])
                    {
                        for (int k = 0; k < colliderObjs[i].GetComponent<PopupInfo>().heldObjTag.Length; k++)
                        {
                            if (heldObjContainer.transform.CompareTag(colliderObjs[i].GetComponent<PopupInfo>().heldObjTag[k]))
                            {
                                lookingAt = "looking at " + colliderObjs[i].name;
                                int popupIndex = colliderObjs[i].GetComponent<PopupInfo>().popupIndex[k];
                                menuController.openPopup(popups[popupIndex]);
                                colliderObjs[i].GetComponent<PopupInfo>().lookingAt[k] = true;

                                for (int j = 0; j < popups.Length; j++)
                                {
                                    if (j != popupIndex)
                                    {
                                        popups[j].SetActive(false);//set all popups to inactive
                                    }
                                }
                                return;
                            }
                        }
                    }
                }//will only get here if nothing returned
                for (int i = 0; i < popups.Length; i++)
                {
                    popups[i].SetActive(false);
                }
            }
            else//close all popups or open popups
            {
                for (int i = 0; i < colliderNonheldObjs.Length; i++)
                {
                    if (hit.transform.gameObject == colliderObjs[i])
                    {
                        lookingAt = "looking at " + colliderObjs[i].name;
                        int popupIndex = colliderObjs[i].GetComponent<PopupInfo>().popupIndex[0];
                        menuController.openPopup(popups[popupIndex]);
                        colliderObjs[i].GetComponent<PopupInfo>().lookingAt[0] = true;

                        for (int j = 0; j < popups.Length; j++)
                        {
                            if (j != popupIndex)
                            {
                                popups[j].SetActive(false);
                            }
                        }
                        return;
                    }
                }//will only get here if nothing returned
                for (int i = 0; i < popups.Length; i++)
                {
                    popups[i].SetActive(false);//set all popups to inactive
                }
            }
        }
    }
}