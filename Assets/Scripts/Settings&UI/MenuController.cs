using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public PlayerMove playerMove;
    public List<GameObject> openMenus;
    public bool menuOpen = false;

    // special function variables:
    public GameObject EscapeMenu;

    /*
     * The menu controller is used to open and close menus. It also handles the cursors lock and visability state.
     * 
     * ---------- How to use ----------
     * Switch buttons to use the menu controller on click events. Then use toggleMenu, closeMenu, or openMenu, ()
     * 
     * ----------    TODO    ----------
     * Intigrate telephone system and backgroud blur.
     * Add popup functionality (pick up prompt, commisions menu etc.)
     */

    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMove.isControllable = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerMove.isControllable = true;
        }

        // key press actions
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleMenu(EscapeMenu);
        }
    }
    // ---------------------------------- Menu Navigation Functions ----------------------------------




    // Open a menu. Closes all other menus.
    public void openMenu(GameObject menuObject)
    {
        GameObject menuToOpen = menuObject;
        // hided all menus
        foreach (GameObject menu in openMenus)
        {
            menu.SetActive(false);

        }
        // show specific menu
        menuToOpen.SetActive(true);
        menuOpen = true;
        // update menu stack
        openMenus.Insert(0, menuToOpen);
    }
    // Closes the top menu in the stack. This is the menu that the user is currently viewing
    public void closeTopMenu()
    {

        if (openMenus.Count > 0)
        {
            openMenus[0].SetActive(false);
            openMenus.RemoveAt(0);
            if (openMenus.Count == 0)
            {
                menuOpen = false;

            }
            else
            {
                openMenus[0].SetActive(true);
            }
        }
        else
        {
            throw new Exception("No top menu to close.");
        }


    }

    // Overload of closeMenu to use the gameObject
    public void closeMenu(GameObject menuObject)
    {
        GameObject menuToClose = menuObject;
        int menuIndex = openMenus.IndexOf(menuToClose); // find the location of the menu in the stack.
        if (menuIndex == -1)
        {
            // we never found the menu :(
            throw new Exception("Menu named " + menuObject.name + " is not open.");
        }
        // close all menus aove this menu.
        for (int i = 0; i < menuIndex + 1; i++)
        {
            // close menus from the top down to the menu we sellected
            closeTopMenu();
        }
    }

    // Overload of toggleMenu to use the GameObject
    public void toggleMenu(GameObject menuObject)
    {
        GameObject menuToToggle = menuObject;
        if (menuToToggle.activeInHierarchy)
        {
            closeMenu(menuToToggle);
        }
        else
        {
            openMenu(menuToToggle);
        }
    }

    // Closes all menus and resets the stack.
    public void closeAllMenus()
    {
        foreach (GameObject menu in openMenus)
        {
            menu.SetActive(false);
        }
        menuOpen = false;
        openMenus.Clear();
    }

    // ---------------------------------- Scene Navigation Functions ----------------------------------

    // switch scene using build index
    public void switchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    // Overload of switchScene for scene name
    public void switchScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}


