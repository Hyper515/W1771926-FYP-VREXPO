using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetToggler : MonoBehaviour
{
    public GameObject assetToToggle;
    public Button button;
    public List<GameObject> buttonsToToggle;
    private bool assetEnabled = false;
    public TMP_Text buttonText;
    private string originalButtonText;

    private void Start()
    {
        if (button != null)
        {
            buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText == null)
            {
                Debug.LogError("Button does not contain Text component!");
                return;
            }

            originalButtonText = buttonText.text;
            UpdateButtonText();
            button.onClick.AddListener(ToggleAssets);
        }
        else
        {
            Debug.LogError("Button reference is not set!");
        }
    }

    private void ToggleAssets()
    {
        assetEnabled = !assetEnabled;
        assetToToggle.SetActive(assetEnabled);

        UpdateButtonText();

        // Toggle other panels
        foreach (GameObject panel in buttonsToToggle)
        {
            panel.SetActive(!assetEnabled);
        }
    }

    private void UpdateButtonText()
    {
        if (assetEnabled)
        {
            buttonText.text = "Return";
        }
        else
        {
            buttonText.text = originalButtonText;
        }
    }

}
