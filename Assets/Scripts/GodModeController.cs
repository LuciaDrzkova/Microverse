using UnityEngine;
using UnityEngine.SpatialTracking;

public class GodModeController : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrRig;          // XR Origin
    public Transform cameraOffset;   // Camera Offset
    public Camera xrCamera;          // Main Camera

    private TrackedPoseDriver poseDriver;

    void Start()
    {
        poseDriver = xrCamera.GetComponent<TrackedPoseDriver>();
    }

    public void EnterGodMode()
    {
        // Disable head rotation tracking
        poseDriver.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;

        // Lock camera to look straight down
        xrCamera.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        // Move rig above the city
        xrRig.position = new Vector3(0f, 30f, 0f);
    }

    public void ExitGodMode()
    {
        // Restore normal XR tracking
        poseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;

        // Reset camera rotation
        xrCamera.transform.localRotation = Quaternion.identity;
    }
}