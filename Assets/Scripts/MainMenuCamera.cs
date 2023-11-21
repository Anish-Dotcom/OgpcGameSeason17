using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainMenuCamera : MonoBehaviour
{
    private Vector3 startRotation;
    private Vector3 targetRotation;
    public float sensitivity;
    public float smoothing = 20;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.eulerAngles;
    }


    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

        

        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;

        mousePos.x = Math.Min(1, mousePos.x);
        mousePos.y = Math.Min(1, mousePos.y);
        mousePos.x = Math.Max(0, mousePos.x);
        mousePos.y = Math.Max(0, mousePos.y);

        mousePos.x -= 0.5f;
        mousePos.y -= 0.5f;


        
        targetRotation = startRotation + (new Vector3(-mousePos.y, mousePos.x, 0) * sensitivity );
        transform.eulerAngles += (targetRotation - transform.eulerAngles) / smoothing;

    }
}
