using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class SelectedOutline : MonoBehaviour
{

    public float baseLinerSpeed = 1;
    public float smoothness = 10;
    public GameObject buttonsContainer;
    private RectTransform buttonsContainerRectTransform;
    public SettingsMenuController settingsMenuController;
    public List<GameObject> buttonsGameObjects;
    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        buttonsContainerRectTransform = buttonsContainer.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainerRectTransform);
        settingsMenuController = GameObject.Find("Settings Menu Controller").GetComponent<SettingsMenuController>();
        canvas = gameObject.GetComponentInParent<Canvas>();
        foreach (Transform child in buttonsContainer.transform)
        {
            buttonsGameObjects.Add(child.gameObject);
        }

    }

    
    private void FixedUpdate()
    {
        MoveRectTransform(transform.GetComponent<RectTransform>(), buttonsGameObjects[settingsMenuController.selected].GetComponent<RectTransform>(), 1 / smoothness);

    }


    public void MoveRectTransform(RectTransform source, RectTransform target, float percent)
    {
        Vector3 difference = target.position - source.position;
        source.position += difference * percent;
    }
}




