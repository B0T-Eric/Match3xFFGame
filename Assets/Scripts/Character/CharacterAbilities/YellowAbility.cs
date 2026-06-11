using System.Collections;
using UnityEngine;
/// <summary>
/// Stun the enemy for 1 turn.
/// cooldown: 1.
/// Cost: 15.
/// </summary>
public class YellowAbility : BaseAbility
{
    public override void ActivateAbility(EnemyCharacter target)
    {
        base.ActivateAbility();
        target.isStunned = true;
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} palm strikes the enemy, stunning them");
    }
    public override string GetDescription()
    {
        return $"Palm Strike:\nStun the enemy for 1 turn." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}