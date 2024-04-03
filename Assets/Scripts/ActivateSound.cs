using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSound : MonoBehaviour
{
    public AudioSource audioSource;
    private bool userInteracted = false;

    // Use OnTriggerEnter for detecting when something enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object interacting with this one has the tag "Player" and the user hasn't interacted yet
        if (other.CompareTag("Player") && !userInteracted)
        {
            // Set the flag to true to indicate user interaction
            userInteracted = true;
            // If the audio source is not playing, start playing it
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
