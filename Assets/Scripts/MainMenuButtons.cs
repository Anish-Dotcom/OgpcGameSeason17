using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour
{
    public float width = 225;
    public float height = 40;
    public float hoverWidth = 250;
    public float animationSpeed = 3;
    public int sceneNumber = 0;
    private bool pointerHovering;
    public void openScene()
    {
        SceneManager.LoadScene(sceneNumber);
  
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void pointerEnter() // used with the event trigger component
    {
        pointerHovering = true;
    }
    public void pointerExit() // used with the event trigger component
    {
        pointerHovering = false;
    }

    void FixedUpdate()
    {
        // find the difference between the width of the button and the target width (depends on if the pointer is hovering over the button)
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        float difference = (pointerHovering ? hoverWidth : width) - rt.sizeDelta.x;
        // resize the button by a faction of the difference. I used fixed update to make this animation run the same speed at all frame rates.
        rt.sizeDelta = new Vector2((float)Math.Round(rt.sizeDelta.x + (difference / animationSpeed)), height);
    }

}
