using UnityEngine;

public class ModeInitializer : MonoBehaviour {
    public GameObject arRoot;
    public GameObject vrRoot;

    void Awake() {
        string mode = PlayerPrefs.GetString("MODE", "AR");
        if (mode == "AR") {
            arRoot.SetActive(true);
            vrRoot.SetActive(false);
        } else {
            arRoot.SetActive(false);
            vrRoot.SetActive(true);
        }
    }
}