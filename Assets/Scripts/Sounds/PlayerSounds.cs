using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] footsteps; 

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
    }

    public void PlayFootstep()
    {
        AudioClip footstepSound = footsteps[Random.Range(0, footsteps.Length)];
        audioSource.PlayOneShot(footstepSound);
    }

}
