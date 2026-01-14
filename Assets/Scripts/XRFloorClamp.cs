using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRFloorClamp : MonoBehaviour
{
    public Transform xrCamera;
    public float minHeadClearance = 0.1f; // prevents head clipping floor

    void LateUpdate()
    {
        if (!FloorManager.Instance || !xrCamera)
            return;

        // Get floor height under the camera
        float floorY = FloorManager.Instance.GetFloorY(xrCamera.position);

        // Calculate camera's local height relative to XR Origin
        float cameraLocalY = xrCamera.localPosition.y;

        // Minimum XR Origin Y so camera stays above floor
        float minOriginY = floorY - cameraLocalY + minHeadClearance;

        Vector3 originPos = transform.position;

        if (originPos.y < minOriginY)
        {
            originPos.y = minOriginY;
            transform.position = originPos;
        }
    }
}
