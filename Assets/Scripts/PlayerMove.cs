using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float objectPickupDistance = 6;
    public float objectHoldDistance = 4;
    public GameObject heldObject;
    public Transform orientation;
    public GameObject cam;

    float horizontalInput;
    float verticalInput;

    private Vector3 newPos;

    Vector3 moveDirection;

    Rigidbody rb;

    public float distToObject = 0;

    public float wrapAroundPosP;
    public float wrapAroundPosN;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        WrapAround();
        PlayerInput();
        SpeedControl();

        if (Input.GetMouseButtonDown(0) && heldObject.IsUnityNull())
        {
            int layermask = -1;
            layermask = layermask & ~(1 << 7);
            Ray grabberRay = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(grabberRay, out hit, objectPickupDistance, layermask))
            {
                distToObject = Vector3.Distance(hit.transform.position, cam.transform.position);

                if (hit.collider.CompareTag("Object"))
                {
                    heldObject = hit.transform.gameObject;
                }
            }
        }

        if (Input.GetMouseButton(0) && !heldObject.IsUnityNull())
        {
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            newPos = cam.transform.position + (cam.transform.forward * objectHoldDistance);
            int layermask = -1;
            layermask = layermask & ~(1 << 7);
            bool canMove = !Physics.BoxCast(heldObject.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.Normalize(newPos - heldObject.transform.position), new Quaternion(0, 0, 0, 0), Vector3.Magnitude(heldObject.transform.position - newPos),layermask);
            if (canMove)
            {
                heldObject.GetComponent<Rigidbody>().transform.position = newPos;
            }
            

        }

        if (!Input.GetMouseButton(0) && heldObject != null)
        {
            heldObject = null;
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void WrapAround()
    {
        float changePosX = 0;
        float changePosZ = 0;
        if (transform.position.x >= 20)
        {
            changePosX = -39;
            Debug.Log("Wrap around x");
        }
        else if (transform.position.x <= -20)
        {
            changePosX = 39;
            Debug.Log("Wrap around -x");
        }
        else if (transform.position.z <= -20)
        {
            changePosZ = 39;
            Debug.Log("Wrap around -z");
        }
        else if (transform.position.z >= 20)
        {
            changePosZ = -39;
            Debug.Log("Wrap around z");
        }
        Vector3 playPos = new Vector3(transform.position.x + changePosX, transform.position.y, transform.position.z + changePosZ);
        transform.position = playPos;
    }
}
