using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverToolTip : MonoBehaviour
{
    public static HoverToolTip Instance { get; private set; }
    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    private TextMeshProUGUI tooltipText;
    [SerializeField]
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private System.Func<string> getTooltipTextFunc;
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        HideToolTip();
    }
    private void ShowToolTip(System.Func<string> getTooltipTextFunc)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
    }
    public static void ShowToolTip_Static(System.Func<string> getTooltipTextFunc)
    {
        Instance.ShowToolTip(getTooltipTextFunc);
    }
    private void ShowToolTip(string tooltipText)
    {
        ShowToolTip(() => tooltipText);
    }
    public static void ShowToolTip_Static(string text)
    {
        Instance.ShowToolTip(text);
    }
    public static void HideTootTip_Static()
    {
        Instance.HideToolTip();
    }
    private void SetText(string text)
    {
        tooltipText.SetText(text);
        tooltipText.ForceMeshUpdate();

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void Update()
    {
        SetText(getTooltipTextFunc());
        Vector2 anchoredPosition = Input.mousePosition / canvas.localScale.x;
        if(anchoredPosition.x + backgroundRectTransform.rect.width > canvas.rect.width)
        {
            anchoredPosition.x = canvas.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvas.rect.height)
        {
            anchoredPosition.y = canvas.rect.height - backgroundRectTransform.rect.height;
        }
        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
