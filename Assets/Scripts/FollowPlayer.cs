using UnityEngine;

public class FollowPlayerCanvas : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 2f;
    public float heightOffset = 0f;
    public float followSpeed = 5f;

    void Update()
    {
        if (!playerCamera) return;

        Vector3 targetPos = playerCamera.position + playerCamera.forward * distance;
        targetPos.y += heightOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        Vector3 lookDir = transform.position - playerCamera.position;
        lookDir.y = 0; // keep it upright
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
