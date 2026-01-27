using System; 
using System.Collections.Generic; 
using UnityEngine; // Represents one object in the city 
[Serializable] 
public class CityObjectData 
{ 
    public string prefabName; 
    public Vector3 position; 
    public Quaternion rotation; 
    public Vector3 scale; 
} 
    // Represents the full city 

[Serializable] 
public class CitySaveData 
{ 
    public List<CityObjectData> objects = new List<CityObjectData>(); 
    }