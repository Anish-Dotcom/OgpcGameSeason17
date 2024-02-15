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
        distFromObject = Vector3.Distance(playerCam.transform.position, transform.position);
        if (lookingAtBox)
        {
            menuController.openPopup(interact);
        } else
        {
            menuController.closePopup(interact);
        }

        if (blueprintIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyUp(KeyCode.Escape))
            {

                menuController.closeMenu(blueprintMenu);
                blueprintIsOpen = false;

            }
        } else if (distFromObject <= 3.5)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                menuController.openMenu(blueprintMenu);
                blueprintIsOpen = true;
            }
        }

    }
    private void OnMouseOver()
    {

        if (distFromObject <= 3.5)
        {
            lookingAtBox = true;
        } else
        {
            lookingAtBox = false;
        }

    }

    private void OnMouseExit()
    {
        lookingAtBox = false;
    }
}