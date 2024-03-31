using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public FatigueController FatigueController;

    public float pullpower = 5;
    public bool crouching = false;

    public static float moveSpeed;
    public float objectPickupDistance = 6;
    public float objectHoldDistance = 4;
    public float maxObjectHoldDistnace = 6;
    public float minObjectHoldDistnace = 2;
    public float currentObjectHoldDistance;
    public GameObject heldObject;
    public Transform orientation;
    public GameObject cam;
    public bool isControllable = true;
    public float wrapAroundPosP;
    public float wrapAroundPosN;

    float horizontalInput;
    float verticalInput;

    private Vector3 newPos;

    Vector3 moveDirection;

    Rigidbody rb;

    public float distToObject = 0;
    Vector3 currentVelocity;
    public float smoothTime = 0.5f;
    public float maxFollowSpeed = 20;
    private float objectStartYDirection = 0;
    private float camStartYDirection = 0;

    public float fatigue;

   PlayerInputs playerInputs;
    public void LoadRebinds()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
        // load rebindings
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) { return; }
        Debug.Log(rebinds);
        playerInputs.LoadBindingOverridesFromJson(rebinds);
    }
    private void Awake()
    {
        LoadRebinds();
    }
    // Start is called before the first frame update
    void Start()
    {


        moveSpeed = 6;
        currentObjectHoldDistance = objectHoldDistance;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        SpeedControl();
        if (isControllable)
        {
            PlayerInput();

            /*if (Input.GetMouseButtonDown(0) && heldObject == null)//pickup object
            {
                int layermask = -1;
                layermask = layermask & ~(1 << 7);
                Ray grabberRay = new Ray(cam.transform.position, cam.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(grabberRay, out hit, objectPickupDistance, layermask))
                {
                    distToObject = Vector3.Distance(hit.transform.position, cam.transform.position);
                    if (hit.collider.CompareTag("Object"))//if pickup-able then pickup
                    {
                        heldObject = hit.transform.gameObject;
                        hit.collider.GetComponent<Rigidbody>().useGravity = false;
                        heldObject.GetComponent<Rigidbody>().drag = 0;
                        objectStartYDirection = hit.collider.GetComponent<Transform>().rotation.eulerAngles.y;
                        camStartYDirection = cam.transform.rotation.eulerAngles.y;
                        Debug.Log("pick up");
                        FatigueController.fatigue+=250;
                        currentObjectHoldDistance = Vector3.Distance(heldObject.transform.position, cam.transform.position);
                    }
                }
            }
            else if (!Input.GetMouseButton(0) && heldObject != null)//put down object
            {
                currentObjectHoldDistance = objectHoldDistance;
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Rigidbody>().drag = 0.1f;
                heldObject = null;
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0f && heldObject != null)//change distance of object from player
            {
                float scrollDirection = Input.GetAxis("Mouse ScrollWheel");
                Gobackandforth(scrollDirection);
            }*/

        }
        
    }

    private void FixedUpdate()//physic based movements
    {

        if (isControllable)
        {
            MovePlayer();
        }
    }

    private void PlayerInput()
    {
   
        horizontalInput = playerInputs.Player.Right.ReadValue<float>() - playerInputs.Player.Left.ReadValue<float>();
        verticalInput = playerInputs.Player.Forward.ReadValue<float>() - playerInputs.Player.Back.ReadValue<float>();

        if (playerInputs.Player.Crouch.WasPressedThisFrame())
        {
            Crouching();
        }
        
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
    }
    
    private void Crouching()
    {
        if(crouching == false)
        {
            transform.localScale = new Vector3 (2,1.2f,2);
            transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);
            moveSpeed = moveSpeed / 2;
            crouching = true;
        }   
        else if (crouching)
        {
            transform.localScale = new Vector3(2, 1.8f, 2);
            transform.position = new Vector3(transform.position.x, 1.8f, transform.position.z);
            moveSpeed = moveSpeed * 2;
            crouching = false;
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
