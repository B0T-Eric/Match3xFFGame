using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusToolTip : ToolTip
{
    BaseStatus status;
    private void Start()
    {
        status = GetComponent<StatusDisplay>().status;
    }
    private void Update()
    {
        Tooltip = $"<b>{status.GetType().Name}</b>\n{status}";
    }
}
