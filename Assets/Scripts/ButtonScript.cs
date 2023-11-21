using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public int sceneToLoad;
    public bool isQuitButton;
    public GameObject objectToActivateOnHover;

    private void OnMouseUpAsButton()
    {
        if (!isQuitButton)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Application.Quit();
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
