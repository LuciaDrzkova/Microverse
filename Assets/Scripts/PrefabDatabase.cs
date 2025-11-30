using System.Collections.Generic;
using UnityEngine;

public class PrefabDatabase : MonoBehaviour {
    public static PrefabDatabase Instance;
    [System.Serializable]
    public struct Entry { public string id; public GameObject prefab; }
    public Entry[] entries;
    private Dictionary<string, GameObject> dict;

    void Awake() {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        dict = new Dictionary<string, GameObject>();
        foreach (var e in entries) {
            if (!string.IsNullOrEmpty(e.id) && e.prefab != null)
                dict[e.id] = e.prefab;
        }
    }

    public GameObject GetPrefabById(string id) {
        return dict.TryGetValue(id, out var p) ? p : null;
    }
}