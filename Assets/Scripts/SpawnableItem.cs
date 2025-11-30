using UnityEngine; // ✅ needed for GameObject, Sprite

[System.Serializable]
public class SpawnableItem
{
    public GameObject prefab;
    public Sprite thumbnail;
    public int groupIndex; // which subcategory button it belongs to
}