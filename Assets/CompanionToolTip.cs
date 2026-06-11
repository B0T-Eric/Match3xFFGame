using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionToolTip : ToolTip
{
    [SerializeField]
    BaseCompanion companion;
    private void Start()
    {
        Tooltip = companion.GetCompanionToolTip();
    }
}
