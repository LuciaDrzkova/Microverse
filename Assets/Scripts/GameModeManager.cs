using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public enum MicroverseMode
{
    Build,
    Explore,
    God
}

public class GameModeManager : MonoBehaviour
{
    [Header("XR Rig")]
    public Transform xrRig;
    public Transform cameraOffset;   // Keep this for Build/Explore modes
    public Camera xrCamera;


    [Header("UI")]
    public GameObject buildUI;
    public GameObject extraUI;

    [Header("Heights")]
    public float buildModeY = 10f;
    public float exploreModeY = 0f;
    public float godModeY = 20f;

    public MicroverseMode currentMode = MicroverseMode.Build;

    private Vector3 cameraOffsetDefaultRotation;
    private TrackedPoseDriver trackedPose;
    private TrackedPoseDriver leftPose;
    private TrackedPoseDriver rightPose;

    [Header("Controllers")]
    public GameObject leftController;
    public GameObject rightController;

    [Header("Controller Roots (Runtime Parents)")]
    public Transform leftControllerRoot;
    public Transform rightControllerRoot;
    public Transform uiRoot;

    private Vector3 leftControllerDefaultPos;
    private Vector3 rightControllerDefaultPos;
    private Quaternion leftControllerDefaultRot;
    private Quaternion rightControllerDefaultRot;
    private Transform leftControllerOriginalParent;
    private Transform rightControllerOriginalParent;


    [Header("God Mode Controller Offset")]
    public Vector3 godControllerOffset = new Vector3(0f, 0f, 0.5f);

    void Start()
    {
        if (cameraOffset != null)
            cameraOffsetDefaultRotation = cameraOffset.localEulerAngles;

        if (xrCamera != null)
            trackedPose = xrCamera.GetComponent<TrackedPoseDriver>();

        if (leftController != null)
            leftPose = leftController.GetComponent<TrackedPoseDriver>();

        if (rightController != null)
            rightPose = rightController.GetComponent<TrackedPoseDriver>();

        leftControllerOriginalParent = leftController.transform.parent;
        rightControllerOriginalParent = rightController.transform.parent;
        
        leftControllerDefaultPos = new Vector3(-0.1f, -0.05f, 0.3f);
        leftControllerDefaultRot = new Quaternion(0f, 0f, 0f, 1f);
        rightControllerDefaultPos = new Vector3(0.1f, -0.05f, 0.3f);
        rightControllerDefaultRot = new Quaternion(0f, 0f, 0f, 1f);

        ApplyMode(currentMode);
    }


    public void ToggleMode()
    {
        currentMode = (MicroverseMode)(((int)currentMode + 1) % 3);
        ApplyMode(currentMode);
    }

    public void SetMode(MicroverseMode mode)
    {
        currentMode = mode;
        ApplyMode(currentMode);
    }

    void ApplyMode(MicroverseMode mode)
    {
        bool showUI = mode == MicroverseMode.Build || mode == MicroverseMode.God;
        if (buildUI) buildUI.SetActive(showUI);
        if (extraUI) extraUI.SetActive(showUI);

        switch (mode)
        {
            case MicroverseMode.Build:
                SetHeadTracking(true);
                Vector3 camPos = xrCamera.transform.localPosition;
                Debug.Log("XR Camera Local Position: " + camPos);
                
                RestoreControllers();
                ResetCameraOffset();

                //if (xrRig != null) xrRig.gameObject.SetActive(true);
                //if (trackedPose != null) trackedPose.enabled = true;
                //if (xrCamera != null) xrCamera.gameObject.SetActive(true);

                SetRigHeight(buildModeY);
                break;

            case MicroverseMode.Explore:
                SetHeadTracking(true);
                RestoreControllers();
                SetRigHeight(exploreModeY);
                ResetCameraOffset();

                break;

            case MicroverseMode.God:
                SetHeadTracking(false);

                leftController.transform.SetParent(leftControllerRoot, true);
                rightController.transform.SetParent(rightControllerRoot, true);

                leftController.transform.localPosition = leftControllerDefaultPos;
                leftController.transform.localRotation = leftControllerDefaultRot;
                rightController.transform.localPosition = rightControllerDefaultPos;
                rightController.transform.localRotation = rightControllerDefaultRot;
                
                leftControllerRoot.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                rightControllerRoot.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                leftControllerRoot.position = xrCamera.transform.position;
                rightControllerRoot.position = xrCamera.transform.position;

                xrCamera.transform.localRotation = Quaternion.Euler(89.9f, 0f, 0f);

                break;
        }

        Debug.Log($"SWITCHED TO MODE: {mode}");
    }

    void SetHeadTracking(bool allowRotation)
    {
        if (leftPose == null || rightPose == null || trackedPose == null)
        {
            Debug.LogError("TrackedPoseDriver missing!");
            return;
        }

        trackedPose.trackingType = allowRotation
            ? TrackedPoseDriver.TrackingType.RotationAndPosition
            : TrackedPoseDriver.TrackingType.PositionOnly;

        leftPose.trackingType = allowRotation
            ? TrackedPoseDriver.TrackingType.RotationAndPosition
            : TrackedPoseDriver.TrackingType.RotationOnly;
        rightPose.trackingType = allowRotation
            ? TrackedPoseDriver.TrackingType.RotationAndPosition
            : TrackedPoseDriver.TrackingType.RotationOnly;

    }

    void LateUpdate()
    {
        
        if (currentMode != MicroverseMode.God) return;

        SetRigHeight(godModeY);

        leftControllerRoot.position = xrCamera.transform.position;
        rightControllerRoot.position = xrCamera.transform.position;

        buildUI.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        extraUI.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
    }


    void RestoreControllers()
    {
        /*
        Vector3 pos = trackedPose.transform.localPosition;
        pos.y = 0f;
        trackedPose.transform.localPosition = pos;

        leftControllerRoot.transform.localPosition = Vector3.zero;
        leftControllerRoot.transform.localRotation = Quaternion.identity;
        rightControllerRoot.transform.localPosition = Vector3.zero;
        rightControllerRoot.transform.localRotation = Quaternion.identity;
        */
        leftController.transform.SetParent(leftControllerOriginalParent, true);
        rightController.transform.SetParent(rightControllerOriginalParent, true);
    
        leftController.transform.localPosition = leftControllerDefaultPos;
        rightController.transform.localPosition = rightControllerDefaultPos;
    }

    void ResetCameraOffset()
    {
        if (cameraOffset != null)
            cameraOffset.localRotation = Quaternion.Euler(cameraOffsetDefaultRotation);
    }

    void SetRigHeight(float y)
    {
        if (xrRig == null) return;
        Vector3 pos = xrRig.position;
        pos.y = y;
        xrRig.position = pos;
    }
}
