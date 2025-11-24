using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScrollhandler : MonoBehaviour
{
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject scrollContent;
    RectTransform scrollRectTransform;
    const float maxY = 500f;
    // Start is called before the first frame update
    void Start()
    {
        scrollRectTransform = scrollContent.GetComponent<RectTransform>();
        scrollRect.onValueChanged.AddListener(HeightLimiter);
    }

    public void HeightLimiter(Vector2 value)
    {
        if (scrollRectTransform == null) return;

        Vector2 anchored = scrollRectTransform.anchoredPosition;
        float clampedY = Mathf.Clamp(anchored.y, -maxY, 0);

        if (!Mathf.Approximately(anchored.y, clampedY))
        {
            anchored.y = clampedY;
            scrollRectTransform.anchoredPosition = anchored;
        }
    }
}
