using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] skyboxes; // Array of skybox materials
    private int currentSkyboxIndex = 0; // Index of the current skybox material

    void Start()
    {
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // Set initial skybox
    }

    public void ToggleSkybox()
    {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length; // Cycle to the next skybox material
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // Set the new skybox
    }
}
