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

        // Ensure all other buttons are visible at start
        foreach (GameObject btn in buttonsToHide)
            btn.SetActive(true);
    }

    // Called by the Info button
    public void OpenInfo()
    {
        infoCanvas.SetActive(true);

        // Hide all other buttons
        foreach (GameObject btn in buttonsToHide)
            btn.SetActive(false);
    }

    // Called by the OK button
    public void CloseInfo()
    {
        infoCanvas.SetActive(false);

        // Show all other buttons again
        foreach (GameObject btn in buttonsToHide)
            btn.SetActive(true);
    }
}