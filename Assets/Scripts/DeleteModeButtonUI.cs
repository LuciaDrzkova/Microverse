using UnityEngine;
using UnityEngine.UI;

public class DeleteModeButtonUI : MonoBehaviour
{
    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color deleteColor = Color.red;

    void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
    }

    void Update()
    {
        if (DeleteModeManager.Instance == null) return;

        // Change the button color based on delete mode state
        buttonImage.color = DeleteModeManager.Instance.deleteMode ? deleteColor : normalColor;
    }
}