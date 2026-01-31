using UnityEngine;
using System.Collections;

public class DemoMusicManager : MonoBehaviour
{
    private static DemoMusicManager instance;
    private static bool hasPlayed = false;

    public AudioSource audioSource;
    public float fadeDuration = 1.5f;

    void Awake()
    {
        // Singleton: destroy duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Play music only once
        if (!hasPlayed && audioSource != null)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }

    // Call this when you want to fade and remove music
    public void FadeOutAndDestroy()
    {
        if (audioSource != null)
            StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        Destroy(gameObject);
        instance = null; // reset singleton so music can be created again
        // DO NOT reset hasPlayed here, so we don't replay music when going back to Demo Scene
    }

    // Reset the music if starting Demo Scene fresh
    public static void ResetMusicFlag()
    {
        hasPlayed = false;
    }
}
