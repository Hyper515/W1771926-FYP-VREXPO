using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectInteraction : MonoBehaviour
{
    public AudioClip interactSound; // Audio clip to play when interacted with

    private AudioSource audioSource;
    private XRNode controllerNode; // The VR controller node (right or left)

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

        // Set the controller node based on the tag of this object
        if (gameObject.CompareTag("LeftController"))
        {
            controllerNode = XRNode.LeftHand;
        }
        else if (gameObject.CompareTag("RightController"))
        {
            controllerNode = XRNode.RightHand;
        }
    }

    void Update()
    {
        // Check for user input on the VR controller
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0)
        {
            Debug.Log("Fire1 Input Detected on VR Controller");
            // Raycast from the controller's position and direction
            RaycastHit hit;
            if (Physics.Raycast(GetControllerPosition(), GetControllerForwardDirection(), out hit))
            {
                Debug.Log("Raycast Hit: " + hit.collider.gameObject.name);
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Object Hit: " + gameObject.name);
                    // If the object is hit by the raycast, play the interact sound
                    Interact();
                }
            }
        }
    }

    // Method to get the position of the VR controller
    Vector3 GetControllerPosition()
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (var nodeState in nodeStates)
        {
            if (nodeState.nodeType == controllerNode)
            {
                Vector3 position;
                if (nodeState.TryGetPosition(out position))
                {
                    return position;
                }
            }
        }

        return Vector3.zero;
    }

    // Method to get the forward direction of the VR controller
    Vector3 GetControllerForwardDirection()
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (var nodeState in nodeStates)
        {
            if (nodeState.nodeType == controllerNode)
            {
                Quaternion rotation;
                if (nodeState.TryGetRotation(out rotation))
                {
                    return rotation * Vector3.forward;
                }
            }
        }

        return Vector3.forward;
    }

    // Method to handle interaction
    void Interact()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            Debug.Log("Playing Interaction Sound...");
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is null!");
        }
    }
}



