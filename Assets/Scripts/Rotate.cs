using UnityEngine;
using UnityEngine.InputSystem;

public class RotateObjectAdvanced : MonoBehaviour
{
    public float smoothRotationSpeed = 90f; // Degrees per sec
    public float snapAngle = 45f;           // Snap rotation amount

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    // RIGHT hand actions
    private InputAction rightStick;
    private InputAction rightStickClick;

    // LEFT hand actions
    private InputAction leftStick;
    private InputAction leftStickClick;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener((_) => isHeld = true);
        grabInteractable.selectExited.AddListener((_) => isHeld = false);
    }

    void Start()
    {
        // RIGHT HAND
        rightStick = new InputAction("RightStick", InputActionType.Value, "<XRController>{RightHand}/primary2DAxis");
        rightStick.Enable();

        rightStickClick = new InputAction("RightStickClick", InputActionType.Button, "<XRController>{RightHand}/primary2DAxisClick");
        rightStickClick.Enable();

        // LEFT HAND
        leftStick = new InputAction("LeftStick", InputActionType.Value, "<XRController>{LeftHand}/primary2DAxis");
        leftStick.Enable();

        leftStickClick = new InputAction("LeftStickClick", InputActionType.Button, "<XRController>{LeftHand}/primary2DAxisClick");
        leftStickClick.Enable();
    }

    void OnDestroy()
    {
        rightStick.Disable();
        rightStickClick.Disable();
        leftStick.Disable();
        leftStickClick.Disable();
    }

    void Update()
    {
        if (!isHeld) return;

        // ---------------- SMOOTH ROTATION ----------------
        Vector2 rStick = rightStick.ReadValue<Vector2>();
        Vector2 lStick = leftStick.ReadValue<Vector2>();

        float rotateInput = 0f;

        if (Mathf.Abs(rStick.x) > 0.1f)
            rotateInput += rStick.x;

        if (Mathf.Abs(lStick.x) > 0.1f)
            rotateInput += lStick.x;

        if (rotateInput != 0f)
        {
            float amount = rotateInput * smoothRotationSpeed * Time.deltaTime;
            transform.Rotate(0f, amount, 0f, Space.World);
        }

        // ---------------- SNAP ROTATION ----------------
        if (rightStickClick.WasPressedThisFrame() || leftStickClick.WasPressedThisFrame())
        {
            transform.Rotate(0f, snapAngle, 0f, Space.World);
        }

        // ---------------- LOCK OUT TILT ----------------
        Vector3 rot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }
}