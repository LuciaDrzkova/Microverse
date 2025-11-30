using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CitySaveManager : MonoBehaviour
{
    [Header("Prefabs Reference")]
    public GameObject[] availablePrefabs; // drag all buildable prefabs here

    [Header("Parent for spawned objects")]
    public Transform spawnedParent; // optional: keep hierarchy clean

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "city.json");
    }

    // ---------------- SAVE ----------------
    public void SaveCity()
    {
        CitySaveData cityData = new CitySaveData();

        // Find all objects under the parent
        foreach (Transform child in spawnedParent)
        {
            // Use prefab name to identify
            CityObjectData data = new CityObjectData();
            data.prefabName = child.gameObject.name.Replace("(Clone)", "").Trim();
            data.position = child.position;
            data.rotation = child.rotation;
            data.scale = child.localScale;

            cityData.objects.Add(data);
        }

        // Convert to JSON
        string json = JsonUtility.ToJson(cityData, true);

        // Save to file
        File.WriteAllText(saveFilePath, json);

        Debug.Log("City saved to: " + saveFilePath);
    }

    // ---------------- LOAD ----------------
    public void LoadCity()
{
    if (!File.Exists(saveFilePath))
    {
        Debug.LogWarning("No saved city found at: " + saveFilePath);
        return;
    }

    // Clear existing spawned objects
    foreach (Transform child in spawnedParent)
        Destroy(child.gameObject);

    // Load JSON
    string json = File.ReadAllText(saveFilePath);
    CitySaveData cityData = JsonUtility.FromJson<CitySaveData>(json);

    // Load each object
    foreach (CityObjectData objData in cityData.objects)
    {
        GameObject prefab = FindPrefabByName(objData.prefabName);

        if (prefab != null)
        {
            // SPAWN THROUGH THE SPAWN MANAGER (adds all components)
            GameObject instance = SpawnManager.Instance.SpawnAndSetup(prefab, objData.position, objData.rotation);

            // Restore transform AFTER spawn
            instance.transform.position = objData.position;
            instance.transform.rotation = objData.rotation;
            instance.transform.localScale = objData.scale;
        }
        else
        {
            Debug.LogWarning("Prefab not found: " + objData.prefabName);
        }
    }

    Debug.Log("City loaded.");
}


    // Helper function to find prefab by name
    private GameObject FindPrefabByName(string name)
    {
        foreach (GameObject prefab in availablePrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }
        return null;
    }
}