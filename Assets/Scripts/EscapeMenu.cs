using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public SettingsMenuController settingsMenuController;
    public GameObject escapeMenu;
    public PlayerMove playerMove;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleEscapeMenu();
            if (settingsMenuController.settingsMenuOpen)
            {
                settingsMenuController.toggleSettingsOpen();
            }

        }
    }

    public void toggleEscapeMenu()
    {
        bool state = !escapeMenu.activeInHierarchy;
        escapeMenu.SetActive(state);
        if (!state)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerMove.isControllable = true;
            
        }
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMove.isControllable = false;
        }
        
    }
    public void quitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void openSettingsMenu()
    {
        settingsMenuController.toggleSettingsOpen();
        escapeMenu.SetActive(false);
    }
    
}
