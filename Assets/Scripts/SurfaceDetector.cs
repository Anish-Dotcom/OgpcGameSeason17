using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    public LayerMask groundLayer; // Layer mask to filter which objects should be considered as ground
    public float minMoveSpeed = 0.1f; // Minimum movement speed to trigger footstep sounds
    public float footstepCooldown = 0.5f; // Cooldown duration between footstep sounds

    private PlayerSounds playerSounds;
    private Rigidbody rb;
    private bool canPlayFootstep = true;

    private void Start()
    {
        playerSounds = GetComponent<PlayerSounds>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > minMoveSpeed && canPlayFootstep)
        {
            DetectSurface();
        }
    }

    private void DetectSurface()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10.0f, groundLayer))
        {

            Debug.Log("goat1");
            if (canPlayFootstep)
            {
                Debug.Log("goat");
                playerSounds.PlayFootstep();
                canPlayFootstep = false;
                Invoke("ResetFootstepCooldown", footstepCooldown);
            }
        }
    }

    private void ResetFootstepCooldown()
    {
        canPlayFootstep = true;
    }
}
