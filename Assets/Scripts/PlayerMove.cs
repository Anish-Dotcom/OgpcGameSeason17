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
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        SpeedControl();

        if (Input.GetMouseButtonDown(0) && heldObject.IsUnityNull())
        {
            Ray grabberRay = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(grabberRay, out hit))
            {
                distToObject = Vector3.Distance(hit.transform.position, cam.transform.position);

                if (distToObject <= objectPickupDistance && hit.collider.CompareTag("Object"))
                {
                    heldObject = hit.transform.gameObject;
                    Debug.Log("pick up");

                }
            }
        }

        if (Input.GetMouseButton(0) && !heldObject.IsUnityNull())
        {
            newPos = cam.transform.position + (cam.transform.forward * objectHoldDistance);
            bool canMove = !Physics.BoxCast(heldObject.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.Normalize(newPos - heldObject.transform.position), new Quaternion(0, 0, 0, 0), Vector3.Magnitude(heldObject.transform.position - newPos));
            if (canMove)
            {
                heldObject.GetComponent<Rigidbody>().transform.position = newPos;
            } else
            {
                Debug.Log("Hit something");
            }
            

        }

        if (!Input.GetMouseButton(0) && heldObject != null)
        {
            Debug.Log("Drop");
            heldObject = null;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(heldObject.transform.position, new Vector3(1, 1, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(newPos, new Vector3(1, 1, 1));
        Ray r = new Ray(heldObject.transform.position, Vector3.Normalize(newPos - heldObject.transform.position));
        Gizmos.DrawRay(r);
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
}
