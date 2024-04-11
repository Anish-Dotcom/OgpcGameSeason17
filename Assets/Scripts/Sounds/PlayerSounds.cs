using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource footsteps;

    public void Update()
    {
        if (transform.hasChanged) {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }
    }
}
