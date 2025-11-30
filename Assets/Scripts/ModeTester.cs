using UnityEngine;
using UnityEngine.InputSystem;

public class ModeTester : MonoBehaviour
{
    public GameModeManager gameModeManager;

    private Keyboard keyboard;

    void Awake()
    {
        keyboard = Keyboard.current;
        if (keyboard == null)
            Debug.LogWarning("No keyboard detected!");
    }

    void Update()
    {
        if (keyboard == null || gameModeManager == null) return;

        // Detect M key press using new Input System
        if (keyboard.mKey.wasPressedThisFrame)
        {
            Debug.Log("M key pressed: switching mode...");
            gameModeManager.ToggleMode();
        }
    }
}
