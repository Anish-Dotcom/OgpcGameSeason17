using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public int sceneToLoad;
    public bool isQuitButton;
    public bool isSettingsButton;
    public SettingsMenuController settingsMenuController;
    public GameObject objectToActivateOnHover;

    public void OnMouseUpAsButton()
    {
        if (isQuitButton)
        {
            Application.Quit();
            
        } else if (isSettingsButton)
        {

            settingsMenuController.toggleSettingsOpen();
        } else
        {
            SceneManager.LoadScene(sceneToLoad);
        }

    }

    private void OnMouseEnter()
    {
        if (objectToActivateOnHover != null)
        {
            objectToActivateOnHover.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (objectToActivateOnHover != null)
        {
            objectToActivateOnHover.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
