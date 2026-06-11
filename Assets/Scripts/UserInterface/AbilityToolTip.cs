using System.Collections;
using UnityEngine;

public class AbilityToolTip : ToolTip
{
    BaseAbility ability;
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.GetComponent<BaseCharacter>();
        ability = character.Ability;
        Tooltip = ability.GetDescription();
    }
}
