using UnityEngine;
using UnityEngine.InputSystem;

public class ModeTester : MonoBehaviour
{
    public GameModeManager gameModeManager;
    private Keyboard keyboard;


    [Header("Input")]
    public InputActionReference flyUpAction; // N (Keyboard) / Oculus Button

    private void OnEnable()
    {
        if (flyUpAction != null)
            flyUpAction.action.Enable();
    }

    private void OnDisable()
    {
        if (flyUpAction != null)
            flyUpAction.action.Disable();
    }

void Awake()
    {
        keyboard = Keyboard.current;
        if (keyboard == null)
            Debug.LogWarning("No keyboard detected!");
    }

    void Update()
    {
        if (gameModeManager == null || flyUpAction == null) return;

        if (flyUpAction.action.WasPressedThisFrame())
        {
            Debug.Log("Fly Up / Mode action triggered (Keyboard or Oculus)");
            gameModeManager.ToggleMode();
        }

        if (keyboard == null || gameModeManager == null) return;

        // Detect M key press using new Input System
        if (keyboard.mKey.wasPressedThisFrame)
        {
            Debug.Log("M key pressed: switching mode...");
            gameModeManager.ToggleMode();
        }
    }
}

