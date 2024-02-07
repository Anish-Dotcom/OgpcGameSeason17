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
        if (lookingAtBox)
        {
            menuController.openPopup(interact);
        } else
        {
            menuController.closePopup(interact);
        }
    }
    private void OnMouseOver()
    {

        
        distFromObject = Vector3.Distance(playerCam.transform.position, transform.position);
        if (distFromObject <= 3.5)
        {
            lookingAtBox = true;
        } else
        {
            lookingAtBox = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            distFromObject = Vector3.Distance(playerCam.transform.position, transform.position);

            if (distFromObject <= 3.5 && !blueprintIsOpen)
            {
                menuController.openMenu(blueprintMenu);
                blueprintIsOpen = true;
            }
            else if (blueprintIsOpen)
            {
                menuController.closeMenu(blueprintMenu);
                blueprintIsOpen = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (blueprintIsOpen)
            {
                menuController.closeMenu(blueprintMenu);
                blueprintIsOpen = false;
            }
        }
    }

    public void OnMouseExit()
    {
        lookingAtBox = false;
    }
}