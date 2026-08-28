using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingToolTip : ToolTip
{
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
    }
    private void Update()
    {
        Tooltip = $"<color=green>Healing Stats</color>\n" +
            $"Mana gain this turn = {Mathf.RoundToInt(character.stats.GetHealing(character.potential))}." +
            $"\nMana gain is how much you heal from a healing action in a turn." +
            $"\nFormula = current potential + flat healing * healing multiplier.";
    }
}
