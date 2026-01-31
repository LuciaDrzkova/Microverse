using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets; // Needed if this is the namespace

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

    [Header("Movement Settings")]
    public Slider movementSpeedSlider;
    public DynamicMoveProvider moveProvider;

    [Header("Buttons to Hide when Settings Open")]
    public GameObject[] buttonsToHide; 

    void Start()
    {
        settingsPanel.SetActive(false);
        // Initialize slider value
            if (audioMixer != null)
            {
                float vol;
                if (audioMixer.GetFloat("MasterVolume", out vol))
                    masterVolumeSlider.value = Mathf.Pow(10, vol / 20f);
            }
            else
            {
                // Check DemoMusic
                DemoMusicManager demoMusic = FindFirstObjectByType<DemoMusicManager>();
                if (demoMusic != null && demoMusic.audioSource != null)
                    masterVolumeSlider.value = demoMusic.audioSource.volume;
                else
                    masterVolumeSlider.value = 1f; // default
            }
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });

        enableHighlightToggle.isOn = true;
        enableHighlightToggle.onValueChanged.AddListener(delegate { OnEnableHighlightsToggled(); });
        OnEnableHighlightsToggled();

        uiScaleSlider.value = 1f;
        uiScaleSlider.onValueChanged.AddListener(delegate { OnUIScaleChanged(); });
        OnUIScaleChanged();

        resetPlayerButton.onClick.AddListener(ResetPlayer);

        rotationSpeedSlider.onValueChanged.AddListener(UpdateRotationSpeed);

        // Movement Speed
        movementSpeedSlider.value = moveProvider.moveSpeed;
        movementSpeedSlider.onValueChanged.AddListener(UpdateMovementSpeed);
    }

    // Open Settings and hide other buttons
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        foreach (GameObject btn in buttonsToHide)
        {
            if (btn != null && btn.activeInHierarchy)
                btn.SetActive(false);
        }
    }

    // Close Settings and restore first 7 buttons
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

           
        
    }

    public void OnMasterVolumeChanged()
{
    // 1️⃣ If AudioMixer exists (Scene 2)
    if (audioMixer != null)
    {
        if (masterVolumeSlider.value <= 0f)
            audioMixer.SetFloat("MasterVolume", -80f);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
    }

    // 2️⃣ If DemoMusic exists (Demo Scene)
    DemoMusicManager demoMusic = FindFirstObjectByType<DemoMusicManager>();
    if (demoMusic != null && demoMusic.audioSource != null)
    {
        demoMusic.audioSource.volume = masterVolumeSlider.value;
    }
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

    public void UpdateMovementSpeed(float value)
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = value;
        }
    }
}