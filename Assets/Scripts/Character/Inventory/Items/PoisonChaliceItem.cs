using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This item applies multiple stacks of poison on attack 
/// if they are currently buffed or debuffed
/// </summary>
public class PoisonChaliceItem : BaseItem, IOnAttackItem, IUpgradableItem
{
    [SerializeField]
    Sprite poisonSprite;
    private int stacksToApply = 3;

    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; }

    public void OnAttack(EnemyCharacter character, int damage)
    {
        if(itemHolder.statusEffects.Count > 0)
        {
            Poison p = new Poison(poisonSprite);
            for (int i = 0; i < stacksToApply; i++)
            {
                StatusHandler.AddStatusEffect(p, character);
            }
        }
    }
    public override string GetEffectDesc()
    {
        return $"Exploit enemies who are already suffering from an ailment (status effect).\n" +
            $"Apply {stacksToApply} stacks of poison on attack.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}" +
            $"\nIncreases a the stacks applied by {UpgradeAmount}.";
    }

    public void Upgrade()
    {
        Tier++;
        stacksToApply += UpgradeAmount;
    }
}
