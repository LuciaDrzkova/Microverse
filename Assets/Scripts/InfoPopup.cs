using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    [Header("References")]
    public GameObject infoCanvas;          // The popup panel
    public GameObject[] buttonsToHide;     // All buttons except the OK button

    void Start()
    {
        // Make sure the info panel is hidden at start
        infoCanvas.SetActive(false);

        // Ensure all other buttons that are active in the scene are visible at start
        foreach (GameObject btn in buttonsToHide)
        {
            if (btn != null && btn.activeInHierarchy)
                btn.SetActive(true);
        }
    }

    // Called by the Info button
    public void OpenInfo()
    {
        infoCanvas.SetActive(true);

        // Hide all other buttons that are active in the scene
        foreach (GameObject btn in buttonsToHide)
        {
            if (btn != null && btn.activeInHierarchy)
                btn.SetActive(false);
        }
    }

    // Called by the OK button
    public void CloseInfo()
    {
        infoCanvas.SetActive(false);

        // Show only the first 7 buttons that exist
        for (int i = 0; i < buttonsToHide.Length && i < 7; i++)
        {
            if (buttonsToHide[i] != null)
                buttonsToHide[i].SetActive(true);
        }
    }
}
