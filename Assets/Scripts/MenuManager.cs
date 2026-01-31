using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public float fadeDuration = 1.5f;

    public void StartAR()
    {
        SceneManager.LoadScene("Demo");
        // We don't touch music here, so DemoMusic keeps playing if exists
    }

    public void StartVR()
    {
        PlayerPrefs.SetString("MODE", "VR");

        // Always fade DemoMusic if it exists
        DemoMusicManager musicManager = FindFirstObjectByType<DemoMusicManager>();
        if (musicManager != null)
        {
            musicManager.FadeOutAndDestroy();
        }

        StartCoroutine(StartVRSequence());
    }

    IEnumerator StartVRSequence()
    {
        // Optional small delay to ensure fade starts
        yield return new WaitForSeconds(0.1f);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync("WorldScene");
        loadOp.allowSceneActivation = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Call this when Demo Scene is loaded fresh to reset music
    public void OnDemoSceneLoaded()
    {
        DemoMusicManager.ResetMusicFlag();
    }
}
