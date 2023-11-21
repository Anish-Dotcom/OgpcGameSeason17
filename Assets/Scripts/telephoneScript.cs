using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telephoneScript : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject telephone;
    public GameObject telephoneOutline;
    public GameObject telephoneUI;
    public GameObject telephoneUIOnPickUp;

    private float moveSpeedSaved;
    private float sensXSaved;
    private float sensYSaved;

    public float distFromObject = 0;

    public bool telephoneIsOpen;

    private void OnMouseUpAsButton()
    {
        distFromObject = Vector3.Distance(playerCam.transform.position, telephone.transform.position);

        if(distFromObject <= 2.1 && !telephoneIsOpen)
        {
            telephoneIsOpen = true;
            telephone.SetActive(false);
            telephoneOutline.SetActive(false);
            telephoneUI.SetActive(true);
            moveSpeedSaved = PlayerMove.moveSpeed;
            PlayerMove.moveSpeed = 0f;
            sensXSaved = PlayerCam.sensX;
            sensYSaved = PlayerCam.sensY;
            PlayerCam.sensX = 0f;
            PlayerCam.sensY = 0f;
            telephoneUIOnPickUp.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PutTelephoneBackButton()
    {
        telephoneIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        telephoneUIOnPickUp.SetActive(false);
        PlayerMove.moveSpeed = moveSpeedSaved;
        PlayerCam.sensX = sensXSaved;
        PlayerCam.sensY = sensYSaved;
        telephone.SetActive(true);
        telephoneOutline.SetActive(true);
        telephoneUI.SetActive(false);
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
