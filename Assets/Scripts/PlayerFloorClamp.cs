using UnityEngine;

public class PlayerFloorClamp : MonoBehaviour
{
    void LateUpdate()
    {
        if (!FloorManager.Instance)
            return;

        float floorY = FloorManager.Instance.GetFloorY(transform.position);

        // Prevent player from going below floor
        if (transform.position.y < floorY)
        {
            Vector3 p = transform.position;
            p.y = floorY;
            transform.position = p;
        }
    }
}
