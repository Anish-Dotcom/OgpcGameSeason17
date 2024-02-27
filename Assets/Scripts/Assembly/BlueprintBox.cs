using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBox : MonoBehaviour
{
    public GameObject interact;
    public GameObject blueprintMenu;
    public bool blueprintIsOpen = false;
    public bool lookingAtBox = false;
    public float distFromObject = 0;
    public GameObject playerCam;
    public MenuController menuController;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//open?
        {
            if (hit.collider.gameObject.CompareTag("blueprintBox"))
            {
                menuController.openPopup(interact);
                lookingAtBox = true;
            }
            else
            {
                menuController.closePopup(interact);
                lookingAtBox = false;
            }
        }

        if (blueprintIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyUp(KeyCode.Escape))
            {

                menuController.closeMenu(blueprintMenu);
                blueprintIsOpen = false;

            }
        } else if (distFromObject <= 3.5 && lookingAtBox)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                menuController.openMenu(blueprintMenu);
                blueprintIsOpen = true;
            }
        }

    }
}