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
            $"Attack damage = {character.GetDamage_Public()}." +
            $"\nAttack damage formula = (potential ({character.potential}) + flat damage ({character.GetFlatDamage_Public()})) * damage multiplier ({character.GetDamageMult_Public()}).";
    }
}
