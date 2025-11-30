using UnityEngine;

public class DeleteModeManager : MonoBehaviour
{
    public static DeleteModeManager Instance;
    public bool deleteMode = false;

    void Awake()
    {
        Instance = this;
    }

    public void ToggleDeleteMode()
    {
        deleteMode = !deleteMode;
        Debug.Log("Delete Mode: " + deleteMode);
    }
}