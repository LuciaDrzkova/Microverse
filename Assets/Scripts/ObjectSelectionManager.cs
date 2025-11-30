using UnityEngine;

public class ObjectSelectionManager : MonoBehaviour
{
    public static ObjectSelectionManager Instance;
    public Transform playerCamera;
    public float selectDistance = 10f;
    public LayerMask interactableLayer;

    private SelectableObject currentSelection;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Simple ray from center of view
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, selectDistance, interactableLayer))
        {
            var selectable = hit.collider.GetComponent<SelectableObject>();
            if (selectable != null && selectable != currentSelection)
            {
                ClearSelection();
                currentSelection = selectable;
                currentSelection.SetSelected(true);
            }

            // Example: click mouse or VR trigger to interact
            if (Input.GetMouseButtonDown(0))
            {
                // Later we’ll add move/scale here
                Debug.Log("Selected: " + currentSelection.name);
            }
        }
        else
        {
            ClearSelection();
        }
    }

    void ClearSelection()
    {
        if (currentSelection != null)
        {
            currentSelection.SetSelected(false);
            currentSelection = null;
        }
    }
}
