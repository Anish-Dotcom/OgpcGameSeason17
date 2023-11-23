using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class telephoneScript : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject telephone;
    public GameObject telephoneOutline;
    public GameObject telephoneUI;
    public GameObject telephoneUIOnPickUp;

    public GameObject callShopButton;
    public GameObject shopUI;
    public AudioSource greeting;

    private float moveSpeedSaved;
    private float sensXSaved;
    private float sensYSaved;

    public float distFromObject = 0;

    public bool telephoneIsOpen;

    public TMP_Text subtitles;

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
        shopUI.SetActive(false);
        callShopButton.SetActive(true);

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

    public void CallShopButton()
    {
        StartCoroutine(showText());
        greeting.Play();
    }

    IEnumerator showText()
    {
        yield return new WaitForSeconds(0.3f);
        subtitles.text = "Thank you for calling Playful Depot.";
        yield return new WaitForSeconds(2.7f);
        subtitles.text = "What would you like to buy?";
        yield return new WaitForSeconds(2.3f);
        subtitles.text = "";
        shopUI.SetActive(true);
        callShopButton.SetActive(false);
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
