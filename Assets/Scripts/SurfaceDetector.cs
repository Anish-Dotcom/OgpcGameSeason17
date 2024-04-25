using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    public float minMoveSpeed = 0.1f; 
    public float footstepCooldown = 0.5f; 

    private PlayerSounds playerSounds;
    private Rigidbody rb;
    private bool canPlayFootstep = true;
    public GlobalDissolveCon GlobalDissolveCon;

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

        if (GlobalDissolveCon.inArea == -1)
        {

            if (canPlayFootstep)
            {
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
