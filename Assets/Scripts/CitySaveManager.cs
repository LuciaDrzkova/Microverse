using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CitySaveManager : MonoBehaviour
{
    [Header("Prefabs Reference")]
    public GameObject[] availablePrefabs; // All possible prefabs you can spawn and save

    [Header("Parent for spawned objects")]
    public Transform spawnedParent; // All saved objects are children of this Transform

    private string savesFolder;

    private void Awake()
    {
    #if UNITY_EDITOR
        savesFolder = Path.Combine(Application.dataPath, "Saves/Cities");
    #else
        savesFolder = Path.Combine(Application.persistentDataPath, "Cities");
    #endif

        if (!Directory.Exists(savesFolder))
            Directory.CreateDirectory(savesFolder);
    }


    // ---------------- SAVE ----------------
   public void SaveCity(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            Debug.LogWarning("City name cannot be empty!");
            return;
        }

        CitySaveData cityData = new CitySaveData();

        foreach (Transform child in spawnedParent)
        {
            CityObjectData data = new CityObjectData();

            // Save the full name including numeric suffix (e.g., "House(2)")
            string childNameFull = child.gameObject.name.Trim();

            // Match the base prefab (without numeric suffix) to validate against availablePrefabs
            string baseName = childNameFull;
            int index = childNameFull.IndexOf('(');
            if (index > 0)
                baseName = childNameFull.Substring(0, index).Trim();

            // Check if this prefab exists in availablePrefabs
            GameObject matchedPrefab = null;
            foreach (GameObject prefab in availablePrefabs)
            {
                if (prefab.name == baseName)
                {
                    matchedPrefab = prefab;
                    break;
                }
            }

            if (matchedPrefab != null)
            {
                data.prefabName = childNameFull; // store full name with number
            }
            else
            {
                data.prefabName = childNameFull; // still store full name even if not in availablePrefabs
                Debug.LogWarning($"Child '{childNameFull}' does not match any prefab in availablePrefabs.");
            }

            data.position = child.position;
            data.rotation = child.rotation;
            data.scale = child.localScale;

            cityData.objects.Add(data);
        }

        string json = JsonUtility.ToJson(cityData, true);
        string filePath = GetCityFilePath(cityName);
        File.WriteAllText(filePath, json);

        Debug.Log($"City '{cityName}' saved to: {filePath}");
    }


    // ---------------- LOAD ----------------
    public void LoadCity(string cityName)
        {
            string filePath = GetCityFilePath(cityName);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No saved city found with name: {cityName}");
                return;
            }

            // Clear current objects
            foreach (Transform child in spawnedParent)
                Destroy(child.gameObject);

            // Read JSON and convert to CitySaveData
            string json = File.ReadAllText(filePath);
            CitySaveData cityData = JsonUtility.FromJson<CitySaveData>(json);

            // Spawn all objects from the saved data
            foreach (CityObjectData objData in cityData.objects)
            {
                string fullName = objData.prefabName.Trim();

                // Extract base prefab name (remove numeric suffix like "(2)")
                string baseName = fullName;
                int index = fullName.IndexOf('(');
                if (index > 0)
                    baseName = fullName.Substring(0, index).Trim();

                // Find prefab in availablePrefabs by base name
                GameObject prefab = FindPrefabByName(baseName);

                if (prefab != null)
                {
                    // Spawn the prefab using your SpawnManager or Instantiate
                    GameObject instance = SpawnManager.Instance.SpawnAndSetup(prefab, objData.position, objData.rotation);

                    // Set exact transform values
                    instance.transform.position = objData.position;
                    instance.transform.rotation = objData.rotation;
                    instance.transform.localScale = objData.scale;

                    // Rename instance to saved full name (with numeric suffix)
                    instance.name = fullName;
                }
                else
                {
                    Debug.LogWarning($"Prefab '{baseName}' not found in availablePrefabs for saved object '{fullName}'.");
                }
            }

            Debug.Log($"City '{cityName}' loaded.");
        }

    // ---------------- LIST SAVED CITIES ----------------
    public List<string> GetSavedCities()
    {
        List<string> cityNames = new List<string>();
        if (!Directory.Exists(savesFolder)) return cityNames;

        string[] files = Directory.GetFiles(savesFolder, "*.json");
        foreach (string file in files)
            cityNames.Add(Path.GetFileNameWithoutExtension(file));

        return cityNames;
    }

    // ---------------- HELPERS ----------------
    public string GetCityFilePath(string cityName)
    {
        return Path.Combine(savesFolder, cityName + ".json");
    }

    public GameObject FindPrefabByName(string name)
    {
        foreach (GameObject prefab in availablePrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }
        return null;
    }
}
