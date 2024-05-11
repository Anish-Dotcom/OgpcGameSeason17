using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialCode : MonoBehaviour
{
    public GameObject popupMenu;

    public GameObject[] tutorials;
    public bool[] happened;

    public GameObject openText;
    public Button close; // close tutorial popup
    public Button left; // view a past popup
    public Button right; // view a popup that will show up eventually

    public MenuController menuController; // for controlling menus

    // Start is called before the first frame update
    void Start()
    {
        numTutorialToHappen(0);
    }

    public void numTutorialToHappen(int num) // this function should be called in the other scripts where if the requirement is met, this will happen
    {
        if(happened[num] == false)
        {
            happened[num] = true;
            setActiveFalse();
            tutorials[num].SetActive(true);
            StartCoroutine(showOpenText());
        }
    }

    IEnumerator showOpenText()
    {
        openText.SetActive(true);
        yield return new WaitForSeconds(5);
        openText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(!popupMenu.activeInHierarchy)
            {
                openText.SetActive(false);
                menuController.openMenu(popupMenu);
            }
            else
            {
                menuController.closeMenu(popupMenu);
            }
        }
    }

    public void setActiveFalse() // resets the tutorial popups and sets them all to false
    {
        foreach(GameObject tutorialObject in tutorials)
        {
            tutorialObject.SetActive(false);
        }
    }

    public void buttonLeft() // view previous tutorial popups in case you want to read something from the past
    {
        right.interactable = true;
        int index = 0;
        for(int i = 0; i < tutorials.Length; i++)
        {
            if(tutorials[i].activeInHierarchy)
            {
                index = i;
            }
        }
        if(index > 0)
        {
            setActiveFalse();
            tutorials[index-1].SetActive(true);
        }
        if(index == 0)
        {
            left.interactable = false;
        }
    }

    public void buttonRight() // see what will show up after what you are viewing the current tuturial popup
    {
        left.interactable = true;
        int index = 0;
        for (int i = 0; i < tutorials.Length; i++)
        {
            if (tutorials[i].activeInHierarchy)
            {
                index = i;
            }
        }
        if (index < 10)
        {
            setActiveFalse();
            tutorials[index+1].SetActive(true);
        }
        if (index == 10)
        {
            right.interactable = false;
        }
    }

    public void closePopup()
    {
        menuController.closeMenu(popupMenu);
    }
}
