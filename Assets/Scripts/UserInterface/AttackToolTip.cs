using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToolTip : ToolTip
{
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
    }
    private void Update()
    {
        Tooltip = $"<color=red>Attack Stats</color>\n" +
            $"Attack damage = {character.stats.GetDamage(character.potential)}." +
            $"\nAttack damage formula = (potential + flat damage * damage multiplier.";
    }
}
