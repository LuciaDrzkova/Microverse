using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    [Header("Assign your floor (like Plane, Terrain, etc.)")]
    public GameObject floorObject;
    private Collider floorCollider;

    private void Awake()
    {
        // Make this a singleton (so others can access it)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (floorObject != null)
            floorCollider = floorObject.GetComponent<Collider>();
    }

    // Finds where the floor is under a given position
    public float GetFloorY(Vector3 worldPos)
    {
        if (!floorCollider)
            return 0f; // Default fallback

        // Cast a ray downward to find the floor
        Vector3 origin = worldPos + Vector3.up * 5f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f))
        {
            if (hit.collider == floorCollider)
                return hit.point.y;
        }

        // Fallback: just return the floor’s Y position
        return floorObject.transform.position.y;
    }
}
