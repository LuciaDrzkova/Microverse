using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private bool isSelected;

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        // Optional: simple visual feedback (change color)
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = selected ? Color.yellow : Color.white;
        }
    }
}
