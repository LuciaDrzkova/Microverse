using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraToFollow;
    public float distance = 1.2f;
    public float heightOffset = 1.3f;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (cameraToFollow == null) return;

        Vector3 targetPos =
            cameraToFollow.position +
            cameraToFollow.forward * distance;
        targetPos.y = cameraToFollow.position.y + heightOffset;

        transform.position = Vector3.Lerp(
            transform.position, targetPos, Time.deltaTime * smooth);

        transform.LookAt(cameraToFollow);
        transform.rotation = Quaternion.Euler(0,
            transform.rotation.eulerAngles.y, 0); // keep upright
    }
}
