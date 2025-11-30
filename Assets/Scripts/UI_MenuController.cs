using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject hudPanel;
    public GameObject categoryPanel;
    public GameObject itemCarouselPanel;

    [Header("Category Buttons")]
    public Button catBuildingsBtn;
    public Button catInfrastructureBtn;
    public Button catPropsBtn;
    public Button catVegetationBtn;
    public Button catVehiclesBtn;

    [Header("Subcategory Buttons Panel")]
    public GameObject subcategoryButtonPanel;
    public List<Button> subButtons; // Assign 10 buttons here

    [Header("Back / Close Buttons")]
    public Button backButton;
    public Button closeCategoryButton;
    public Button closeCarouselButton;

    [Header("Show Categories Button")]
    public Button showCategoriesButton;

    [Header("Category Groups")]
    // Buildings - 4 groups
    public List<SpawnableItem> buildingsGroup1 = new List<SpawnableItem>();
    public List<SpawnableItem> buildingsGroup2 = new List<SpawnableItem>();
    public List<SpawnableItem> buildingsGroup3 = new List<SpawnableItem>();
    public List<SpawnableItem> buildingsGroup4 = new List<SpawnableItem>();

    // Infrastructure - 3 groups
    public List<SpawnableItem> infraGroup1 = new List<SpawnableItem>();
    public List<SpawnableItem> infraGroup2 = new List<SpawnableItem>();
    public List<SpawnableItem> infraGroup3 = new List<SpawnableItem>();

    // Vehicles - 3 groups
    public List<SpawnableItem> vehicleGroup1 = new List<SpawnableItem>();
    public List<SpawnableItem> vehicleGroup2 = new List<SpawnableItem>();
    public List<SpawnableItem> vehicleGroup3 = new List<SpawnableItem>();

    // Props & Vegetation (simple categories)
    public List<SpawnableItem> propsItems = new List<SpawnableItem>();
    public List<SpawnableItem> vegetationItems = new List<SpawnableItem>();

    [Header("Carousel Manager")]
    public CarouselManager carouselManager;

    [Header("Carousel ScrollRect")]
    public ScrollRect carouselScrollRect;

    private string currentCategory = "";

    void Start()
    {
        categoryPanel.SetActive(false);
        itemCarouselPanel.SetActive(false);
        subcategoryButtonPanel.SetActive(false);

        // Hook category buttons
        catBuildingsBtn.onClick.AddListener(() => OpenCategory("Buildings"));
        catInfrastructureBtn.onClick.AddListener(() => OpenCategory("Infrastructure"));
        catVehiclesBtn.onClick.AddListener(() => OpenCategory("Vehicles"));
        catPropsBtn.onClick.AddListener(() => OpenSimpleCategory(propsItems));
        catVegetationBtn.onClick.AddListener(() => OpenSimpleCategory(vegetationItems));

        // Back / Close
        backButton.onClick.AddListener(BackToCategories);
        closeCategoryButton.onClick.AddListener(CloseAll);
        closeCarouselButton.onClick.AddListener(CloseAll);

        if (showCategoriesButton != null)
            showCategoriesButton.onClick.AddListener(ShowCategories);
    }

    // Show categories from HUD button
    public void ShowCategories()
    {
        categoryPanel.SetActive(true);
        itemCarouselPanel.SetActive(false);
        subcategoryButtonPanel.SetActive(false);

        showCategoriesButton.gameObject.SetActive(false);
    }

    // Open category
    void OpenCategory(string category)
    {
        currentCategory = category;
        subcategoryButtonPanel.SetActive(true);

        List<List<SpawnableItem>> groups = new List<List<SpawnableItem>>();

        switch (category)
        {
            case "Buildings":
                groups.Add(buildingsGroup1);
                groups.Add(buildingsGroup2);
                groups.Add(buildingsGroup3);
                groups.Add(buildingsGroup4);
                break;
            case "Infrastructure":
                groups.Add(infraGroup1);
                groups.Add(infraGroup2);
                groups.Add(infraGroup3);
                break;
            case "Vehicles":
                groups.Add(vehicleGroup1);
                groups.Add(vehicleGroup2);
                groups.Add(vehicleGroup3);
                break;
        }

        // Populate carousel with **all items from all groups**
        List<SpawnableItem> combined = new List<SpawnableItem>();
        foreach (var g in groups)
            combined.AddRange(g);

        carouselManager.PopulateFromList(combined);
        ResetScroll();

        SetupSubButtons(groups, category);
        
        categoryPanel.SetActive(false);
        itemCarouselPanel.SetActive(true);
    }

    // Setup subcategory buttons
    void SetupSubButtons(List<List<SpawnableItem>> groups, string category)
{
    // Hide all buttons first
    foreach (var btn in subButtons)
    {
        btn.gameObject.SetActive(false);
        btn.onClick.RemoveAllListeners();
        btn.image.color = Color.white;
    }

    int startIndex = 0;

    // Assign ranges
    switch (category)
    {
        case "Infrastructure":
            startIndex = 0; // use subButtons 0,1,2
            break;

        case "Buildings":
            startIndex = 3; // use 3,4,5,6
            break;

        case "Vehicles":
            startIndex = 7; // use 7,8,9
            break;
    }

    // Activate correct number of buttons from the starting index
    for (int i = 0; i < groups.Count; i++)
    {
        int btnIndex = startIndex + i;

        subButtons[btnIndex].gameObject.SetActive(true);

        int index = i; // closure for group
        subButtons[btnIndex].onClick.AddListener(() => SelectSubgroup(groups[index], subButtons[btnIndex]));
    }
}

    void SelectSubgroup(List<SpawnableItem> list, Button btn)
    {
        carouselManager.PopulateFromList(list);
        ResetScroll();
        HighlightButton(btn);
    }

    void HighlightButton(Button btn)
    {
        foreach (var b in subButtons) b.image.color = Color.white;
        btn.image.color = Color.green;
    }

    void ResetScroll()
    {
        if (carouselScrollRect != null)
            carouselScrollRect.horizontalNormalizedPosition = 0.5f;
    }

    void OpenSimpleCategory(List<SpawnableItem> list)
    {
        subcategoryButtonPanel.SetActive(false);
        carouselManager.PopulateFromList(list);
        ResetScroll();
        categoryPanel.SetActive(false);
        itemCarouselPanel.SetActive(true);
    }

    public void BackToCategories()
    {
        itemCarouselPanel.SetActive(false);
        subcategoryButtonPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }

    public void CloseAll()
    {
        categoryPanel.SetActive(false);
        itemCarouselPanel.SetActive(false);
        subcategoryButtonPanel.SetActive(false);
        showCategoriesButton.gameObject.SetActive(true);
    }
}