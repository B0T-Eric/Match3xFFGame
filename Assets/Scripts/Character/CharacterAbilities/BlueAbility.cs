using System.Collections;
using UnityEngine;
/// <summary>
/// Blue Ability: Trap enemy in a bubble, which increases damage dealt by the next attack, or detonates for damage at the start of the next turn.
/// Cost: 20 mana.
/// Cooldown: 1 turns;
/// </summary>
public class BlueAbility : BaseAbility
{
    [SerializeField]
    Sprite bubbleSprite;
    public override void ActivateAbility(EnemyCharacter target)
    {
        base.ActivateAbility();
        Bubble b = new(bubbleSprite, target);
        StatusHandler.AddStatusEffect(b, target);
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} used Bubble Trap");
    }
    public override string GetDescription()
    {
        return $"Bubble Trap:\nEntrap the enemy in a bubble, preventing their next turn." +
            $"\nOn the next turn or when the enemy is attacked next the bubble pops." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}