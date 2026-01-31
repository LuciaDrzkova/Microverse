using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CitySaveUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject savePanel;
    public TMP_InputField cityNameInput;
    public Button confirmButton;
    public Button cancelButton;

    [Header("Save Manager")]
    public CitySaveManager citySaveManager;

    [Header("Buttons to Hide when Save Panel Opens")]
    public GameObject[] buttonsToHide;

    private void Start()
    {
        savePanel.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirmSave);
        cancelButton.onClick.AddListener(OnCancelSave);
    }

    public void ShowSavePanel()
    {
        cityNameInput.text = "";

        // Hide all active buttons in the list
        foreach (GameObject btn in buttonsToHide)
        {
            if (btn != null && btn.activeInHierarchy)
                btn.SetActive(false);
        }

        savePanel.SetActive(true);
    }

    private void OnConfirmSave()
    {
        string cityName = cityNameInput.text.Trim();
        if (string.IsNullOrEmpty(cityName))
        {
            Debug.LogWarning("City name cannot be empty!");
            return;
        }

        citySaveManager.SaveCity(cityName);
        savePanel.SetActive(false);
        // Buttons remain hidden
    }

    private void OnCancelSave()
    {
        savePanel.SetActive(false);
        // Buttons remain hidden
    }
}
