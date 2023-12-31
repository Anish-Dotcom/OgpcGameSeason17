using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuController : MonoBehaviour
{
    public int selected = 1;
    public MainMenuCamera MainMenuCameraScript;
    public GameObject settingsPageContainer;
    public GameObject settingsGameObject;
    public List<GameObject> settingsPages;
    public PlayerMove playerMove;
    public PlayerCam playerCameraScript;
    public bool isMainGame = false;
    public bool settingsMenuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in settingsPageContainer.transform)
        {
            settingsPages.Add(child.gameObject);
            
        }
        updatePage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void updatePage()
    {
        int index = 0;
        foreach (GameObject page in settingsPages)
        {
            page.SetActive(index == selected);
            index++;
        }
    }

    public void toggleSettingsOpen()
    {
        bool originalState = settingsGameObject.activeInHierarchy;
        settingsMenuOpen = !settingsMenuOpen;
        settingsGameObject.SetActive(!originalState);
        if (isMainGame)
        {
            if (originalState)
            {
                playerCameraScript.updateSenstivity();
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                playerMove.isControllable = true;
            }
            
        } else
        {
            if (originalState)
            {
                MainMenuCameraScript.pointToTable();
            }
            else
            {
                MainMenuCameraScript.pointToCabnet();
            }
        }

    }
}
