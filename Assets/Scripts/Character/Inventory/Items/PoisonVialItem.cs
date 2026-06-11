using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonVialItem : BaseItem, IOnTurnEnd, IUpgradableItem
{
    Sprite poisonSprite;
    int stacksToApply = 1;
    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; }
    public void OnTurnEnd()
    {
        Poison p = new Poison(poisonSprite);
        for (int i = 0; i < stacksToApply; i++)
        {
            StatusHandler.AddStatusEffect(p, EnemyCharacter.Instance);
        }
    }
    public override string GetEffectDesc()
    {
        return $"Apply {stacksToApply} stack poison to the enemy at end of turn.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}" +
            $"\nIncreases stacks applied by {UpgradeAmount} per tier.";

    }
    public void Upgrade()
    {
        Tier++;
        stacksToApply += UpgradeAmount;
    }
}
