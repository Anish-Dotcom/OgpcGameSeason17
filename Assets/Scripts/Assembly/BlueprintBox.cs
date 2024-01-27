using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBox : MonoBehaviour
{
    public GameObject interact;
    public GameObject blueprintMenu;
    public bool blueprintIsOpen = false;
    public float distFromObject = 0;
    public GameObject playerCam;

    public float moveSpeedSaved;
    public float sensXSaved;
    public float sensYSaved;


    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            distFromObject = Vector3.Distance(playerCam.transform.position, transform.position);

            if (distFromObject <= 3.5 && !blueprintIsOpen)
            {
                blueprintMenu.SetActive(true);
                blueprintIsOpen = true;
                moveSpeedSaved = PlayerMove.moveSpeed;//used so when you exit menu you can set back to previous, prob better to just have this saved somewhere else and you can only change current move speed, but still have saved move speed, same with sense x & y
                PlayerMove.moveSpeed = 0f;
                sensXSaved = PlayerCam.sensX;
                sensYSaved = PlayerCam.sensY;
                PlayerCam.sensX = 0f;
                PlayerCam.sensY = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    private void OnMouseOver()
    {
        distFromObject = Vector3.Distance(playerCam.transform.position, transform.position);
        //Debug.Log(distFromObject);

        if (distFromObject <= 3.5 && !blueprintIsOpen)
        {
            Debug.Log("open blueprint?");
            interact.SetActive(true);
        }
        else
        {
            Debug.Log("set active false");
            interact.SetActive(false);
        }
    }
    public void CloseBlueprintMenu()
    {
        blueprintIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMove.moveSpeed = moveSpeedSaved;
        PlayerCam.sensX = sensXSaved;
        PlayerCam.sensY = sensYSaved;
        blueprintMenu.SetActive(false);
    }
}