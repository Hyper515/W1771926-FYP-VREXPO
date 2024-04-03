using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSound : MonoBehaviour
{
    public AudioSource audioSource;

    private void Update()
    {
        // Check if Fire1 input or primary button on the Oculus controller is pressed
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0)
        {
            // If the audio source is not playing, start playing it
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}