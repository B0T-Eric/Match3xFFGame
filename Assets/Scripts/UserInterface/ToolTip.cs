using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [TextArea, SerializeField]
    string Tooltip;
    private void OnMouseEnter()
    {
        System.Func<string> getTooltipTextFunc = () =>
        {
            return Tooltip;
        };
        HoverToolTip.ShowToolTip_Static(getTooltipTextFunc);
    }
    private void OnMouseExit()
    {
        HoverToolTip.HideTootTip_Static();
    }
}
