using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;

    public Transform orientation;
    public GameObject cam;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public float distToObject;
    public bool pickedUp = false;

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

        if (Input.GetMouseButtonDown(0)&&!pickedUp)
        {
            Ray grabberRay = new Ray(transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(grabberRay, out hit, distToObject))
            {

                Debug.Log("ray");
                if (hit.collider.CompareTag("Object"))
                {
                    Debug.Log("hit");
                    Vector3 newPos = cam.transform.position + cam.transform.forward * distToObject;
                    hit.transform.position = newPos;
                    pickedUp = true;
                }
            }
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
}
