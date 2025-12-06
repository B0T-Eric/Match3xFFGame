using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlagueItem : DraggableItem, IOnTurnEnd, IOnTurnStart, IUpgradableItem
{
    public int Tier {  get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; }
    private int AdditionalDamage = 0;
    public void OnTurnEnd()
    {
        Poison poison = EnemyCharacter.Instance?.statusEffects?.OfType<Poison>().FirstOrDefault();
        if(poison != null )
        {
            EnemyCharacter.Instance.LoseMana(poison.StackCount + AdditionalDamage);
        }
    }

    public void OnTurnStart()
    {
        foreach (BaseStatus status in EnemyCharacter.Instance.statusEffects) 
        {
            if (status is Poison poison)
            {
                poison.DecreaseStack();
            }
        }
    }
    public void Upgrade()
    {
        Tier++;
        AdditionalDamage += UpgradeAmount;
    }
    public override string GetEffectDesc()
    {
        return $"At the end of each turn, deal damage equal to the stacks of poison affecting the enemey." +
            $"\nAt the start of each turn, lower the amount of poison on the enemy by 1.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}." +
            $"\nEach tier provides {UpgradeAmount} additional damage (not per stack) to this effect.";
    }
}
