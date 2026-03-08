using System.Collections;
using UnityEngine;
/// <summary>
/// purple character ability taunts the enemy, making their targets collection only equal this character.
/// cooldown: 2.
/// Cost: 5.
/// </summary>
public class PurpleAbility : BaseAbility 
{

    public override void ActivateAbility(BaseCharacter target)
    {
        base.ActivateAbility();
        EnemyCharacter enemy = (EnemyCharacter)target;
        enemy.targets.Clear();
        enemy.targets.Add(character);
    }
}
