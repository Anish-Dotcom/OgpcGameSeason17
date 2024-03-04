using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public FatigueController FatigueController;

    public float pullpower = 5;

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
    public float[] maxDistances = new float[4];
    private float objectStartYDirection = 0;
    private float camStartYDirection = 0;

    public float fatigue;

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
        WrapAround();
        
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
            if (heldObject != null)//change object position
            {
                MoveHeldObject();
            }
        }
    }

    private void Gobackandforth(float Direction)
    {
        Vector3 scrollOffset = cam.transform.forward * Direction * pullpower * 5f;
        newPos = heldObject.transform.position + scrollOffset;

        int layermask = -1;
        layermask = layermask & ~(1 << 7);
        bool canMove = !Physics.BoxCast(heldObject.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.Normalize(scrollOffset), new Quaternion(0, 0, 0, 0), Vector3.Magnitude(scrollOffset), layermask);

        if (canMove)
        {
            // keep distance in range
            currentObjectHoldDistance += Direction * pullpower;
            currentObjectHoldDistance = Math.Min(Math.Max(currentObjectHoldDistance, minObjectHoldDistnace), maxObjectHoldDistnace);
        }

    }

    private void OnDrawGizmos()
    {
        if (heldObject != null)
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawCube(heldObject.transform.position, new Vector3(1, 1, 1));
            Gizmos.color = Color.red;
            //Gizmos.DrawCube(newPos, new Vector3(1, 1, 1));
            Ray r = new Ray(heldObject.transform.position, Vector3.Normalize(newPos - heldObject.transform.position));
            //Gizmos.DrawRay(r);
        }
    }


    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey("Shift"))
        {

        }

    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
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
    private void MoveHeldObject()
    {
        heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        newPos = cam.transform.position + (cam.transform.forward* currentObjectHoldDistance);
        int layermask = -1;
        layermask = layermask & ~(1 << 7);
        //bool canMove = !Physics.BoxCast(heldObject.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.Normalize(newPos - heldObject.transform.position), new Quaternion(0, 0, 0, 0), Vector3.Magnitude(heldObject.transform.position - newPos), layermask);
        //if (canMove)
        //{
        //
        //}
        heldObject.GetComponent<Rigidbody>().transform.position = Vector3.SmoothDamp(heldObject.GetComponent<Rigidbody>().transform.position, newPos, ref currentVelocity, smoothTime, maxFollowSpeed);

        heldObject.transform.eulerAngles = new Vector3 (heldObject.transform.eulerAngles.x, objectStartYDirection + (cam.transform.rotation.eulerAngles.y - camStartYDirection), heldObject.transform.eulerAngles.z);

    }
    private void WrapAround()
    {
        float changePosX = 0;
        float changePosZ = 0;
        if (transform.position.x >= maxDistances[0])
        {
            changePosX = -2 * transform.position.x + 0.5f;
            Debug.Log("Wrap around x" + transform.position.x);
        }
        else if (transform.position.x <= maxDistances[1])
        {
            changePosX = -2 * transform.position.x - 0.5f;
            Debug.Log("Wrap around -x" + transform.position.x);
        }
        else if (transform.position.z <= maxDistances[2])
        {
            changePosZ = -2 * transform.position.z - 0.5f;
            Debug.Log("Wrap around -z" + transform.position.z);
        }
        else if (transform.position.z >= maxDistances[3])
        {
            changePosZ = -2 * transform.position.z + 0.5f;
            Debug.Log("Wrap around z" + transform.position.z);
        }
        Vector3 playPos = new Vector3(transform.position.x + changePosX, transform.position.y, transform.position.z + changePosZ);
        transform.position = playPos;
        //Debug.Log(transform.position.x + transform.position.z);
    }
}
