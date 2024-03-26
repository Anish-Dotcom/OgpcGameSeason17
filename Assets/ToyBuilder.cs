using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBuilder : MonoBehaviour
{


    public GameObject heldObjContainer;

    public bool lookingAt = false;
    public float distFromPlayer;
    public GameObject playerCam;

    public MenuController menuController;
    public GameObject addAssembly;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//add assembly?
        {
            if (heldObjContainer.transform.childCount > 0)
            {
                if (heldObjContainer.transform.GetChild(0).CompareTag("assemblyPart"))
                {
                    lookingAt = true;
                    menuController.openPopup(addAssembly);
                }
                else
                {
                    lookingAt = false;
                    menuController.closePopup(addAssembly);
                }
            }
            else
            {
                lookingAt = false;
                menuController.closePopup(addAssembly);
            }
        }
        if (lookingAt)
        {
            if (Input.GetKeyDown(KeyCode.E))//add the object command
            {
                AddToBuilder();
                ObjectPickUp.equipped = false;
                ObjectPickUp.slotFull = false;
            }
            else if (Input.GetKeyDown(KeyCode.F))//enter build mode
            {
                EnterBuildMode();
            }
        }
    }
    public void AddToBuilder()
    {

    }
    public void EnterBuildMode()
    {

    }
}
