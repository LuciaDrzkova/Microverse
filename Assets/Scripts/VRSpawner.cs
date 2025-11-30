using UnityEngine;

public class VRSpawner : MonoBehaviour {
    public GameObject[] prefabs;       // building prefabs in inspector
    public Transform spawnPoint;       // assign SpawnPoint (child of camera)
    public Transform cityParent;       // assign CityRoot

    public void SpawnSelected(int index) {
        if (index < 0 || index >= prefabs.Length) return;
        GameObject go = Instantiate(prefabs[index], spawnPoint.position, spawnPoint.rotation, cityParent);
        // Optional: push it slightly forward so it doesn't intersect the camera
        go.transform.position = spawnPoint.position + spawnPoint.forward * 0.2f;
    }
}