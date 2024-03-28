using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchPubliPrivateMenu : MonoBehaviour
{
    public GameObject objectToDeactivate;
    public GameObject objectToActivate;
    public TMP_Text titleText;
    public TMP_Text buttonText;
    public Button toggleButton;

    private bool canvasActive = false;

    void Start()
    {
        // Set up initial button text
        UpdateButtonText();
        UpdateTitleText();
    }

    public void ToggleObject()
    {
        canvasActive = !canvasActive;
        objectToDeactivate.SetActive(!canvasActive);
        objectToActivate.SetActive(canvasActive);

        // Update button text and title text accordingly
        UpdateButtonText();
        UpdateTitleText();
    }

    private void UpdateButtonText()
    {
        if (canvasActive)
            buttonText.text = "Public";
        else
            buttonText.text = "Private";
    }

    private void UpdateTitleText()
    {
        if (canvasActive)
            titleText.text = "Private Rooms";
        else
            titleText.text = "Public Rooms";
    }
}
