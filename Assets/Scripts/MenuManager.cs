using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void StartAR() {
        //PlayerPrefs.SetString("MODE", "AR");
        SceneManager.LoadScene("Demo");
    }
    public void StartVR() {
        PlayerPrefs.SetString("MODE", "VR");
        SceneManager.LoadScene("WorldScene");
    }
     public void QuitGame()
    {
#if UNITY_EDITOR
        // Stop play mode inside the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
    }
}