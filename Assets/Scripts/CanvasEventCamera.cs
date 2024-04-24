using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEventCamera : MonoBehaviour
{
    public Canvas[] canvases; // Array of Canvas objects
    public Camera eventCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Main Camera in the scene
        eventCamera = Camera.main;

        if (eventCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene.");
        }
        else
        {
            Debug.Log("Main Camera found: " + eventCamera.name);
            // Now you can use 'eventCamera' variable as needed
        }

        // Assign the event camera to each canvas in the array
        foreach (Canvas canvas in canvases)
        {
            if (canvas != null)
            {
                canvas.worldCamera = eventCamera;
            }
            else
            {
                Debug.LogWarning("Canvas reference is null.");
            }
        }
    }
}
