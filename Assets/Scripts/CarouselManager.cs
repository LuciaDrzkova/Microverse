using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform contentParent;   // your Content RectTransform
    public GameObject buttonPrefab;       // UI button prefab

    // This list is optional for default population, but we'll use runtime lists
    [Header("Optional Default Prefabs (editable)")]
    public List<GameObject> defaultPrefabs = new List<GameObject>();

    void Start()
    {
        // optional: populate default on Start if desired
        // PopulateFromList(defaultPrefabs);
    }

    // Public API: populate the scroll content with buttons that spawn items in the list
    public void PopulateFromList(List<SpawnableItem> items)
{
    if (contentParent == null || buttonPrefab == null)
    {
        Debug.LogWarning("CarouselManager: contentParent or buttonPrefab missing.");
        return;
    }

    // Clear existing children
    for (int i = contentParent.childCount - 1; i >= 0; i--)
        Destroy(contentParent.GetChild(i).gameObject);

    if (items == null || items.Count == 0)
    {
        Debug.Log("CarouselManager: no items to populate.");
        return;
    }

    foreach (var item in items)
    {
        GameObject newBtn = Instantiate(buttonPrefab, contentParent);

        // Assign image
        Image img = newBtn.GetComponentInChildren<Image>();
        if (img != null && item.thumbnail != null)
            img.sprite = item.thumbnail;

        // Assign text (optional)
        Text txt = newBtn.GetComponentInChildren<Text>();
        if (txt != null)
            txt.text = item.prefab != null ? item.prefab.name : "NULL";

        // Hook click event
        Button b = newBtn.GetComponent<Button>();
        if (b != null && item.prefab != null)
        {
            GameObject prefabRef = item.prefab;
            b.onClick.AddListener(() =>
            {
                SpawnManager.Instance.RequestSpawn(prefabRef);
            });
        }
    }
}
}
