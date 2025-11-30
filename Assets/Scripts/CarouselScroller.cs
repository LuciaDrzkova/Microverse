using UnityEngine;
using UnityEngine.UI;

public class CarouselScroller : MonoBehaviour
{
    public ScrollRect scrollRect;      // assign ItemCarouselPanel ScrollRect
    public float scrollStep = 0.16f;   // adjust based on number of items per page
    private float target = 0f;

    public void ScrollLeft()
    {
        target = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - scrollStep);
        scrollRect.horizontalNormalizedPosition = target;
    }

    public void ScrollRight()
    {
        target = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + scrollStep);
        scrollRect.horizontalNormalizedPosition = target;
    }
}