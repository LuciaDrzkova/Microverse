using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDemo : MonoBehaviour
{
    public void LoadDemoScene()
    {
        SceneManager.LoadScene("Demo");
    }
}