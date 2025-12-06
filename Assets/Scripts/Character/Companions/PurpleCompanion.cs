using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// After paired character has taken 15 damage deal 1/3rd the damage taken and heal back 1/9th the damage taken
/// </summary>
public class PurpleCompanion : BaseCompanion
{
    int threshold = 15;
    int damageToCharacter = 2;
    bool active = false;
    public override void PreformAction()
    {
        if(pairedCharacter.damageTaken >= 15 && !active)
        {
            active = true;
        }
        if(active)
        {
            pairedCharacter.HealMana(Mathf.RoundToInt(pairedCharacter.damageTaken / 3));
            EnemyCharacter.Instance.LoseMana(Mathf.RoundToInt(pairedCharacter.damageTaken / 9),companionName);
        }
        else
        {
            pairedCharacter.LoseMana(damageToCharacter, companionName);
        }
    }
    public override void Init(BaseCharacter c)
    {
        base.Init(c);
    }
    protected override string GetDescription()
    {
        return $"When {pairedCharacter.GetCharacterName()} takes more than {threshold} damage, this companion activates." +
            $"\nDealing damage (one third the damage taken) to the enemy and heal the character (one ninth the damage taken)." +
            $"\nWhen inactive, {pairedCharacter.GetCharacterName()} loses 2 spirit mana.";
    }
}
