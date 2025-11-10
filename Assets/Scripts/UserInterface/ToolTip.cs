using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea, SerializeField]
    protected string Tooltip;
    
    protected virtual void HideToolTip()
    {
        HoverToolTip.HideTootTip_Static();
    }
    protected virtual void DisplayToolTip(string? tooltip)
    { 
        System.Func<string> getTooltipTextFunc = () =>
        {
            return Tooltip;
        };
        HoverToolTip.ShowToolTip_Static(getTooltipTextFunc);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayToolTip(Tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }
}
