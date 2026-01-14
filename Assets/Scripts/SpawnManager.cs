using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("References")]
    public Transform playerCamera;
    public float spawnDistance = 2f;
    public float groundY = 0f;
    public Transform spawnedParent;

    [Header("Audio")]
    public AudioClip spawnSound;        // Assign the sound in the Inspector
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Add AudioSource if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void RequestSpawn(GameObject prefab)
    {
        Vector3 pos = playerCamera.position + playerCamera.forward * spawnDistance;
        pos.y = groundY;

        GameObject newObj = SpawnAndSetup(prefab, pos, Quaternion.identity);

        // Play spawn sound
        PlaySpawnSound();
    }

    public GameObject SpawnAndSetup(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject newObj = Instantiate(prefab, pos, rot, spawnedParent);

        // Force upright
        newObj.transform.rotation = Quaternion.identity;

        // XRGrabInteractable
        var grab = newObj.GetComponent<XRGrabInteractable>();
        if (grab == null) grab = newObj.AddComponent<XRGrabInteractable>();
        grab.movementType = XRBaseInteractable.MovementType.Kinematic;
        grab.trackPosition = true;
        grab.trackRotation = false;
        grab.throwOnDetach = false;

        // Rigidbody
        Rigidbody rb = newObj.GetComponent<Rigidbody>();
        if (rb == null) rb = newObj.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // Add scripts
        if (!newObj.GetComponent<SnapToGround>()) newObj.AddComponent<SnapToGround>();
        if (!newObj.GetComponent<RotateObjectAdvanced>()) newObj.AddComponent<RotateObjectAdvanced>();
        if (!newObj.GetComponent<SelectableObject>()) newObj.AddComponent<SelectableObject>();
        if (!newObj.GetComponent<DeletableObject>()) newObj.AddComponent<DeletableObject>();

        return newObj;
    }

    private void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }
}
