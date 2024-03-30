using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBox : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject blueprintMenu;
    public bool blueprintIsOpen = false;
    public MenuController menuController;

    void Update()
    {
        if (blueprintIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyUp(KeyCode.Escape))
            {

                menuController.closeMenu(blueprintMenu);
                blueprintIsOpen = false;

            }
        } else if (lookingAtCheck.lookingAt[myInfoIndex])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                menuController.openMenu(blueprintMenu);
                blueprintIsOpen = true;
            }
        }
    }
}