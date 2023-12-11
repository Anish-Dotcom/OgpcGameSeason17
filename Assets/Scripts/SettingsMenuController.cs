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
    public bool isMainGame = false;
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
        if (Input.GetKeyDown(KeyCode.Space) && isMainGame)
        {
            if (settingsGameObject.activeInHierarchy)
            {
                settingsGameObject.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            } else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                settingsGameObject.SetActive(true);
            }
            
        }
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
        settingsGameObject.SetActive(!originalState);
        if (originalState && !isMainGame)
        {
            MainMenuCameraScript.pointToTable();
        } else
        {
            MainMenuCameraScript.pointToCabnet();
        }
    }
}
