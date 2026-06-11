using System.Collections;
using UnityEngine;

/// <summary>
/// White ability is a free attack, useable once every 2 turns
/// cost: 10.
/// cooldown: 2.
/// </summary>
public class WhiteAbility : BaseAbility
{
    [SerializeField]
    BasicAttack attack1;
    public override void ActivateAbility(EnemyCharacter target)
    {
        base.ActivateAbility();
        int originalAttackIndex = character.selectedAttack;
        character.selectedAttack = character.attacks.IndexOf(attack1);
        character.Attack(target);
        character.selectedAttack = originalAttackIndex;
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} strikes the enemy with a basic attack");
    }
    public override string GetDescription()
    {
        return $"Advantage Strike:\nPreforms a single basic attack." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}
