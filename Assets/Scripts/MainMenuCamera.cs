using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainMenuCamera : MonoBehaviour
{

    public Vector3 targetRotation;
    public Vector3 targetLocation;
    public Vector3 tableLocation;
    public Vector3 tableRotation;
    public Vector3 cabnetLocation;
    public Vector3 cabnetRotation;
    public float sensitivity;
    public float rotationSmoothing = 20;
    public float transformSmoothing = 20;
    // Start is called before the first frame update
    void Start()
    {
        pointToTable();
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
        Vector3 mouseAjustedTargetRotation = targetRotation + (new Vector3(-mousePos.y, mousePos.x, 0) * sensitivity);
        transform.eulerAngles += FindRealDifference(transform.rotation.eulerAngles, mouseAjustedTargetRotation) / rotationSmoothing;
        transform.position += (targetLocation - transform.position) / transformSmoothing;
    }

    public void pointToTable()
    {
        targetLocation = tableLocation;
        targetRotation = tableRotation;
    }
    public void pointToCabnet()
    {
        targetLocation = cabnetLocation;
        targetRotation = cabnetRotation;
    }


    private Vector3 FindRealDifference(Vector3 v1, Vector3 v2)
    {
        Vector3 difference;
        difference.x = FindRealDifference(v1.x, v2.x);
        difference.y = FindRealDifference(v1.y, v2.y);
        difference.z = FindRealDifference(v1.z, v2.z);
        return difference;
    }
    private float FindRealDifference(float d1, float d2)
    {
        float difference = d2 - d1;
        if (difference > 180)
        {
            difference -= 360;
        }
        if (difference < -180) 
        {
            difference += 360;
        }
        return difference;
    }
}
