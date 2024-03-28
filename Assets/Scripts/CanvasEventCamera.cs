using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEventCamera : MonoBehaviour
{
    public Canvas canvas;
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
            // Now you can use 'mainCamera' variable as needed
        }

        // Assign the event camera to the canvas
        canvas.worldCamera = eventCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
