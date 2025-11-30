using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VRSettings : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public Slider masterVolumeSlider;
    public Toggle enableHighlightToggle;  
    public Slider uiScaleSlider;           
    public Button resetPlayerButton;       

    [Header("UI Elements to Scale")]
    public RectTransform[] uiElements;     

    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("XR Rig")]
    public Transform xrRig;                
    public Transform spawnPoint;           

    [Header("Build Mode / Highlight")]
    public GameObject[] placeableObjects;  

    [Header("Rotation Settings")]
    public Slider rotationSpeedSlider;

    void Start()
    {
        settingsPanel.SetActive(false);

        float vol;
        if (audioMixer.GetFloat("MasterVolume", out vol))
            masterVolumeSlider.value = Mathf.Pow(10, vol / 20f);
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });

        enableHighlightToggle.isOn = true;
        enableHighlightToggle.onValueChanged.AddListener(delegate { OnEnableHighlightsToggled(); });
        OnEnableHighlightsToggled();

        uiScaleSlider.value = 1f;
        uiScaleSlider.onValueChanged.AddListener(delegate { OnUIScaleChanged(); });
        OnUIScaleChanged();

        resetPlayerButton.onClick.AddListener(ResetPlayer);

        rotationSpeedSlider.onValueChanged.AddListener(UpdateRotationSpeed);
    }

    public void OpenSettings() => settingsPanel.SetActive(true);
    public void CloseSettings() => settingsPanel.SetActive(false);

    public void OnMasterVolumeChanged()
    {
        if (masterVolumeSlider.value <= 0f)
            audioMixer.SetFloat("MasterVolume", -80f);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
    }

    public void OnEnableHighlightsToggled()
    {
        bool enable = enableHighlightToggle.isOn;

        foreach (var obj in placeableObjects)
        {
            if (obj != null)
            {
                Transform highlight = obj.transform.Find("Highlight");
                if (highlight != null)
                    highlight.gameObject.SetActive(enable);
            }
        }
    }

    public void OnUIScaleChanged()
    {
        float scale = uiScaleSlider.value;
        settingsPanel.transform.localScale = Vector3.one * scale;
    }

    public void ResetPlayer()
    {
        if (xrRig != null && spawnPoint != null)
        {
            xrRig.position = spawnPoint.position;
            xrRig.rotation = spawnPoint.rotation;
        }
    }

    public void UpdateRotationSpeed(float value)
    {
        foreach (var obj in Object.FindObjectsByType<RotateObjectAdvanced>(FindObjectsSortMode.None))
        {
            obj.smoothRotationSpeed = value;
        }
    }
}