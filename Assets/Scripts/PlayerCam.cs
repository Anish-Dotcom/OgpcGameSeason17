using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject playerCamera;
    public Transform cameraPos;

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRot;
    float yRot;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Senscript.sense != 0) 
        {
            sensX = Senscript.sense;
            sensY = Senscript.sense;
        }
        else 
        {
            sensX = 400f;
            sensY = 400f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.position = cameraPos.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
