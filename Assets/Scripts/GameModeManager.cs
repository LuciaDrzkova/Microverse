using UnityEngine;


public enum MicroverseMode { Build, Explore }

public class GameModeManager : MonoBehaviour
{
    [Header("XR Interactors")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor leftRay;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRay;

    [Header("UI Canvases")]
    public GameObject buildUI;    // carousel / build menu
    public GameObject extraUI;    // secondary canvas

    [Header("Player Rig")]
    public Transform xrRig;       // assign XR Origin / Rig
    public float buildModeY = 10f; // height when in Build Mode
    public float exploreModeY = 0f;

    [Header("Current Mode")]
    public MicroverseMode currentMode = MicroverseMode.Build;

    void Start()
    {
        ApplyMode(currentMode);
    }

    /// <summary>
    /// Toggle between Build and Explore modes
    /// </summary>
    public void ToggleMode()
    {
        currentMode = (currentMode == MicroverseMode.Build)
            ? MicroverseMode.Explore
            : MicroverseMode.Build;

        ApplyMode(currentMode);
    }

    /// <summary>
    /// Force set mode directly
    /// </summary>
    public void SetMode(MicroverseMode mode)
    {
        currentMode = mode;
        ApplyMode(currentMode);
    }

    /// <summary>
    /// Apply the current mode settings
    /// </summary>
    void ApplyMode(MicroverseMode mode)
    {
        bool build = mode == MicroverseMode.Build;

        // XR Rays: enable/disable by GameObject to ensure visuals work
       // if (leftRay) leftRay.gameObject.SetActive(build);
       // if (rightRay) rightRay.gameObject.SetActive(build);

        // UI
        if (buildUI) buildUI.SetActive(build);
        if (extraUI) extraUI.SetActive(build);

        // XR Rig height
        if (xrRig != null)
        {
            Vector3 pos = xrRig.position;
            pos.y = build ? buildModeY : exploreModeY;
            xrRig.position = pos;
        }

        Debug.Log($"SWITCHED TO MODE: {mode}");
        //Debug.Log($"LeftRay Active: {leftRay?.gameObject.activeSelf}");
        //Debug.Log($"RightRay Active: {rightRay?.gameObject.activeSelf}");
        Debug.Log($"BuildUI Active: {buildUI?.activeSelf}");
        Debug.Log($"ExtraUI Active: {extraUI?.activeSelf}");
    }
}
