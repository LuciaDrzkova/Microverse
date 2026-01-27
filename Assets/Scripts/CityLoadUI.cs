using UnityEngine;
 using UnityEngine.UI;
  using TMPro;
  using System.Collections.Generic; 
  using System.IO;

public class CityLoadUI : MonoBehaviour 
{ [Header("UI References")] 
public GameObject loadPanel; 

// LoadCityPanel 
public Transform contentParent;
 // Scroll View -> Content
 public GameObject cityButtonPrefab;
  // Button prefab with DeleteButton inside 
public Button cancelButton; 
[Header("Save Manager")] 
public CitySaveManager citySaveManager; 

private void Start() 
{ loadPanel.SetActive(false); 
if (cancelButton != null) 
cancelButton.onClick.AddListener(HideLoadPanel); 
else Debug.LogWarning("Cancel button not assigned in CityLoadUI"); } 

public void ShowLoadPanel() 
{ ClearList(); 
if (cityButtonPrefab == null) 
{ Debug.LogError("CityButtonPrefab is not assigned!"); 
return; } 
List<string> savedCities = citySaveManager.GetSavedCities();
 if (savedCities.Count == 0) 
 { Debug.Log("No saved cities found."); 
 return; } 
 foreach (string cityName in savedCities) 
 { GameObject btnObj = Instantiate(cityButtonPrefab, contentParent); 
 btnObj.SetActive(true); 
 
 // Set TMP text for the city name 
 TMP_Text text = btnObj.GetComponentInChildren<TMP_Text>(); 
 if (text != null) text.text = cityName; 
 // Load button 
 Button loadBtn = btnObj.GetComponent<Button>(); 
 if (loadBtn != null) 
 { string nameCopy = cityName; 
loadBtn.onClick.AddListener(() => LoadCity(nameCopy)); }
 // Delete button
  Button deleteBtn = btnObj.transform.Find("DeleteButton")?.GetComponent<Button>(); 
  if (deleteBtn != null)
   { string nameCopy = cityName; deleteBtn.onClick.AddListener(() => DeleteCity(nameCopy)); } }
   
    loadPanel.SetActive(true); } 
    
    private void LoadCity(string cityName)
     { citySaveManager.LoadCity(cityName); 
     loadPanel.SetActive(false); } 
     
     private void DeleteCity(string cityName) 
     { string filePath = citySaveManager.GetCityFilePath(cityName); 
     if (File.Exists(filePath))
     { File.Delete(filePath);
      Debug.Log($"City '{cityName}' deleted."); } 
      else { Debug.LogWarning($"City '{cityName}' file not found for deletion."); } 
      // Refresh the list after deletion 
      ShowLoadPanel(); } 
      
    private void HideLoadPanel() 
    { loadPanel.SetActive(false); } 
    
    private void ClearList() 
    { foreach (Transform child in contentParent) 
    { Destroy(child.gameObject); } } }