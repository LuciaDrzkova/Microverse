using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CitySaveManager : MonoBehaviour
{
    [Header("Prefabs Reference")]
    public GameObject[] availablePrefabs;

    [Header("Parent for spawned objects")]
    public Transform spawnedParent;

    private string savesFolder;

    private void Awake()
    {
        savesFolder = Path.Combine(Application.persistentDataPath, "Cities");
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
            data.prefabName = child.gameObject.name.Replace("(Clone)", "").Trim();
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

        foreach (Transform child in spawnedParent)
            Destroy(child.gameObject);

        string json = File.ReadAllText(filePath);
        CitySaveData cityData = JsonUtility.FromJson<CitySaveData>(json);

        foreach (CityObjectData objData in cityData.objects)
        {
            GameObject prefab = FindPrefabByName(objData.prefabName);

            if (prefab != null)
            {
                GameObject instance = SpawnManager.Instance.SpawnAndSetup(prefab, objData.position, objData.rotation);
                instance.transform.position = objData.position;
                instance.transform.rotation = objData.rotation;
                instance.transform.localScale = objData.scale;
            }
            else
            {
                Debug.LogWarning("Prefab not found: " + objData.prefabName);
            }
        }

        Debug.Log($"City '{cityName}' loaded.");
    }

    // ---------------- LIST SAVED CITIES ----------------
    public List<string> GetSavedCities()
    {
        List<string> cityNames = new List<string>();

        if (!Directory.Exists(savesFolder))
            return cityNames;

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
