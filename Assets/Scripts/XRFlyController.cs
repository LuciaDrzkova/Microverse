using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRFlyController : MonoBehaviour
{
    [Header("References")]
    public Transform xrRig;                 // XR Rig
    public VRSettings vrSettings;           // Drag your VRSettings script here

    [Header("Input Actions")]
    public InputActionReference flyUpAction;   // e.g., N button
    public InputActionReference flyDownAction; // e.g., B button

    void Update()
    {
        if (xrRig == null || vrSettings == null) return;

        // Read fly speed from VRSettings slider (same as move speed)
        float speed = vrSettings.movementSpeedSlider.value;

        Vector3 vertical = Vector3.zero;

        if (flyUpAction != null && flyUpAction.action.ReadValue<float>() > 0.1f)
            vertical.y += speed * Time.deltaTime;

        if (flyDownAction != null && flyDownAction.action.ReadValue<float>() > 0.1f)
            vertical.y -= speed * Time.deltaTime;

        xrRig.position += vertical;
    }
}