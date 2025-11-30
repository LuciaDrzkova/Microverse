using UnityEngine;

public class XRDeletableObject : MonoBehaviour
{
    // Call this function to delete the object
    public void Delete()
    {
        Destroy(gameObject);
        Debug.Log("Deleted object: " + gameObject.name);
    }
}