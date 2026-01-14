using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this

public class CitySaveUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject savePanel;
    public TMP_InputField cityNameInput; // <- Change from InputField to TMP_InputField
    public Button confirmButton;
    public Button cancelButton;

    public CitySaveManager citySaveManager;

    private void Start()
    {
        savePanel.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirmSave);
        cancelButton.onClick.AddListener(OnCancelSave);
    }

    public void ShowSavePanel()
    {
        cityNameInput.text = "";
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
    }

    private void OnCancelSave()
    {
        savePanel.SetActive(false);
    }
}
