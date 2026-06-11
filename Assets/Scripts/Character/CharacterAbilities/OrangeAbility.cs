using System.Collections;
using UnityEngine;

/// <summary>
/// High damage meteor that deals 100 total damage across 2 instances, 1 on cast and the other one burn trigger.
/// cooldown: 2 turns.
/// cost: 20 mana.
/// </summary>
public class OrangeAbility : BaseAbility
{
    [SerializeField]
    int duration;
    [SerializeField]
    int damage;
    [SerializeField]
    Sprite icon;
    public override void ActivateAbility(EnemyCharacter target)
    {
        base.ActivateAbility();
        Burn B = new(duration, damage, target, icon);
        target.LoseMana(damage,this.GetType().Name);
        StatusHandler.AddStatusEffect(B,target);
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} summoned a meteor on {target.GetCharacterName()}");
    }
    public override string GetDescription()
    {
        return $"Meteor:\nSummon a meteorite which deals {damage} and applies burn to the enemy." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}
