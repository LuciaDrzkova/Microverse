using UnityEngine;

public class DemoSceneInitializer : MonoBehaviour
{
    void Start()
{
    MenuManager menuManager = FindFirstObjectByType<MenuManager>();
    if(menuManager != null)
        menuManager.OnDemoSceneLoaded();
}

}
