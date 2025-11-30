using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DeletableObject : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Listen for the grab event
        grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (DeleteModeManager.Instance.deleteMode)
        {
            Debug.Log("Deleted via grab: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}