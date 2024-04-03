using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSound : MonoBehaviour
{
    public AudioSource audioSource;

    // Use OnTriggerEnter for detecting when something enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object interacting with this one has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // If the audio source is not playing, start playing it
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
