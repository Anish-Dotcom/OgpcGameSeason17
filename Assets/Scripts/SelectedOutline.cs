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
        MoveRectTransformToAnother(transform.GetComponent<RectTransform>(), buttonsGameObjects[settingsMenuController.selected].GetComponent<RectTransform>(), canvas, 1 / smoothness);

    }

    public void MoveRectTransformToAnother(RectTransform source, RectTransform target, Canvas canvas, float percent)
    {
        Vector3 targetWorldPosition = target.TransformPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, targetWorldPosition), canvas.worldCamera, out Vector2 sourceLocalPosition);
        sourceLocalPosition.x = 0; // idk this just works
        Vector2 difference = sourceLocalPosition - source.anchoredPosition;
        source.anchoredPosition += difference * percent;
    }
}




