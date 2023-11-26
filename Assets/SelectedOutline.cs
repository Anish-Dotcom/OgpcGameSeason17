using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class SelectedOutline : MonoBehaviour
{

    public int selected = 0;
    public float baseLinerSpeed = 1;
    public float smoothness = 10;
    public GameObject buttonsContainer;
    public GameObject categoryButtonsLocation;
    public List<GameObject> buttonsGameObjects;
    public Vector2[] buttonLocations;
    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponentInParent<Canvas>();
        StartCoroutine(init());
        
    }

    IEnumerator init()
    {
        yield return new WaitForSeconds(0.01f); // have to wait because the vertical layout groups doesn't update on start :(

        int length = 0;
        foreach (Transform child in buttonsContainer.transform)
        {
            buttonsGameObjects.Add(child.gameObject);
            length++;
        }
        buttonLocations = new Vector2[length];

        for (int i = 0; i < length; i++)
        {

            RectTransform buttonRectTransform = buttonsGameObjects[i].GetComponent<RectTransform>();
            Vector3 worldPos = buttonRectTransform.transform.TransformPoint(buttonRectTransform.position);
            buttonLocations[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPos);
            buttonLocations[i] = new Vector2((float)Math.Round(buttonLocations[i].x), (float)Math.Round(buttonLocations[i].y));

        }
        
    }



    private void FixedUpdate()
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position);
        float difference = buttonLocations[selected].y - (float)Math.Round(screenPosition.y);
        difference += Math.Sign(difference) * baseLinerSpeed; // this is to make sure that it is possible to reach the target value
        if (Math.Abs(difference) < 0.5)
        {
            transform.localPosition += new Vector3(0, difference, 0);
        } else
        {
            transform.localPosition += new Vector3(0, (float)Math.Round(difference / smoothness), 0);
        }

        

    }



}
