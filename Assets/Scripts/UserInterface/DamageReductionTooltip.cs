using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReductionTooltip : ToolTip
{
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
    }
    private void Update()
    {
        Tooltip = $"<color=grey>Defence Stats</color>\n" +
            $"Damage Reduction = {character.GetDamageReduction_Public()}." +
            $"\nDamage reduction lowers damage taken by the specified amount." +
            $"\nFormula = Damage Incoming - Damage Reduction.";
    }
}
