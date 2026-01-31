using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class SnapToGround : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    [Header("Ground Settings")]
    public float groundY = 0f;
    public float snapOffsetY = 0.1f;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Kinematic by default
        rb.isKinematic = true;
        rb.useGravity = false;

        // Prevent throwing physics
        grabInteractable.throwOnDetach = false;

        // Listen for release
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Snap to ground
        Vector3 pos = transform.position;
        pos.y = groundY + snapOffsetY;
        transform.position = pos;

        // KEEP user rotation, only flatten X/Z tilt
        Vector3 rot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);

        // Keep kinematic
        rb.isKinematic = true;
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}