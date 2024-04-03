using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public AudioClip interactSound; // Audio clip to play when interacted with

    private AudioSource audioSource;

    void Start()
    {
        // Check if there's an AudioSource attached to the object
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // If no AudioSource is attached, create one and configure it
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the interactSound to the AudioSource clip if it's not null
        if (interactSound != null)
        {
            audioSource.clip = interactSound;
            // Ensure that audio doesn't play on awake
            audioSource.playOnAwake = false;
        }
        else
        {
            Debug.LogWarning("No interactSound assigned to InteractableObject script on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check for user input (Fire1 command)
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0)
        {
            // Raycast from the center of the screen in VR
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // If the object is hit by the raycast, play the interact sound
                    if (audioSource.clip != null)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }
}

