using UnityEngine;

using System.Collections.Generic;

public class DeleteGrabbedObject : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor leftHand;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor rightHand;

    void Update()
    {
        TryDelete(leftHand);
        TryDelete(rightHand);
    }

    void TryDelete(UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor hand)
    {
        if (hand == null) return;
        if (!DeleteModeManager.Instance.deleteMode)
            return;

        // ⬇⬇⬇ Your version returns List<IXRSelectInteractable>
        List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable> grabbedList = hand.interactablesSelected;

        if (grabbedList != null && grabbedList.Count > 0)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable grabbed = grabbedList[0];

            // Convert to GameObject
            GameObject grabbedObj = grabbed.transform.gameObject;

            if (grabbedObj.GetComponent<DeletableObject>() != null)
            {
                Debug.Log("Deleted object: " + grabbedObj.name);
                Destroy(grabbedObj);
            }
        }
    }
}