using UnityEngine;

public class CameraStraightLineAccelerating : MonoBehaviour
{
    [Header("Line Settings")]
    public Transform startPoint;       // Start near the city
    public Transform endPoint;         // End far away
    public bool faceBackward = false;  // Look toward city or along movement
    public bool loop = false;          // Loop back?

    [Header("Speed Settings")]
    public float startSpeed = 20f;     // Initial speed
    public float maxSpeed = 50f;       // Maximum speed
    public float acceleration = 5f;    // Units per second^2

    private Vector3 moveDirection;
    private float currentSpeed;

    void Start()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("Start or End point not assigned!");
            enabled = false;
            return;
        }

        transform.position = startPoint.position;

        moveDirection = (endPoint.position - startPoint.position).normalized;

        // Initial speed
        currentSpeed = startSpeed;

        // Set rotation along the movement direction
        Vector3 forwardDir = faceBackward ? -moveDirection : moveDirection;
        transform.rotation = Quaternion.LookRotation(forwardDir, Vector3.up);
    }

    void Update()
    {
        // Move camera
        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        // Accelerate toward max speed
        currentSpeed += acceleration * Time.deltaTime;
        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;

        // Check if reached end
        if (Vector3.Distance(transform.position, endPoint.position) < 0.01f)
        {
            if (loop)
            {
                // Swap points for looping
                Vector3 temp = startPoint.position;
                startPoint.position = endPoint.position;
                endPoint.position = temp;

                moveDirection = (endPoint.position - startPoint.position).normalized;

                // Update rotation
                Vector3 forwardDir = faceBackward ? -moveDirection : moveDirection;
                transform.rotation = Quaternion.LookRotation(forwardDir, Vector3.up);

                // Reset speed
                currentSpeed = startSpeed;
            }
            else
            {
                transform.position = endPoint.position;
                enabled = false;
            }
        }
    }
}