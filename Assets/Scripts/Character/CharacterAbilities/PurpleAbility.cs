using System.Collections;
using UnityEngine;
/// <summary>
/// purple character ability taunts the enemy, making their targets collection only equal this character.
/// cooldown: 2.
/// Cost: 5.
/// </summary>
public class PurpleAbility : BaseAbility 
{

    public override void ActivateAbility(EnemyCharacter target)
    {
        base.ActivateAbility();
        target.targets.Clear();
        //if wolf remove all wolf targets
        if(target is WolfEnemy wolves)
        {
            foreach(Wolf w in wolves.WolfPack)
            {
                w.target = character;
            }
        }
        target.targets.Add(character);
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} taunts the enemy to attack them.");
    }
    public override string GetDescription()
    {
        return $"Taunt:\n" +
            $"Sets the enemy targets to this character." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}
