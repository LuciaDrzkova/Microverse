using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CollapsibleUI : MonoBehaviour
{
    [Header("Panels to Hide (RectTransforms)")]
    public List<RectTransform> panelsToHide = new List<RectTransform>(); // multiple panels

    [Header("Toggle")]
    public Button toggleButton;

    [Header("Sliding")]
    public float slideDistance = 0.4f; // meters (World Space) or pixels (Screen Space)
    public float slideSpeed = 8f;

    [Header("Options")]
    public bool moveToggleButton = true;
    public RectTransform toggleButtonRect;

    private bool isCollapsed = false;

    // Store original and hidden positions for all panels
    private List<Vector3> ws_originalPos = new List<Vector3>();
    private List<Vector3> ws_hiddenPos = new List<Vector3>();

    private List<Vector2> ss_originalPos = new List<Vector2>();
    private List<Vector2> ss_hiddenPos = new List<Vector2>();

    private List<Canvas> parentCanvases = new List<Canvas>();

    void Awake()
    {
        if (toggleButton != null)
            toggleButton.onClick.AddListener(TogglePanel);

        if (toggleButtonRect == null && toggleButton != null)
            toggleButtonRect = toggleButton.GetComponent<RectTransform>();
    }

    void Start()
    {
        foreach (var panel in panelsToHide)
        {
            Canvas canvas = panel.GetComponentInParent<Canvas>();
            parentCanvases.Add(canvas);

            // WORLD SPACE CANVAS
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                Vector3 original = panel.localPosition;
                Vector3 hidden = original + new Vector3(slideDistance, 0, 0);

                ws_originalPos.Add(original);
                ws_hiddenPos.Add(hidden);

                ss_originalPos.Add(Vector2.zero);
                ss_hiddenPos.Add(Vector2.zero);
            }
            // SCREEN SPACE
            else
            {
                Vector2 original = panel.anchoredPosition;
                Vector2 hidden = original + new Vector2(slideDistance, 0);

                ss_originalPos.Add(original);
                ss_hiddenPos.Add(hidden);

                ws_originalPos.Add(Vector3.zero);
                ws_hiddenPos.Add(Vector3.zero);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < panelsToHide.Count; i++)
        {
            RectTransform panel = panelsToHide[i];
            Canvas canvas = parentCanvases[i];

            // WORLD SPACE
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                Vector3 target = isCollapsed ? ws_hiddenPos[i] : ws_originalPos[i];
                panel.localPosition = Vector3.Lerp(panel.localPosition, target, Time.deltaTime * slideSpeed);
            }
            // SCREEN SPACE
            else
            {
                Vector2 target = isCollapsed ? ss_hiddenPos[i] : ss_originalPos[i];
                panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, target, Time.deltaTime * slideSpeed);
            }
        }

        // Rotate toggle icon
        if (toggleButton != null)
            toggleButton.transform.localRotation = isCollapsed ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
    }

    void TogglePanel()
    {
        isCollapsed = !isCollapsed;

        // Move toggle slightly when collapsed
        if (moveToggleButton && toggleButtonRect != null)
        {
            toggleButtonRect.localPosition += isCollapsed
                ? new Vector3(0.2f, 0, 0)
                : new Vector3(-0.2f, 0, 0);
        }
    }
}