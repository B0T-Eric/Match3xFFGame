using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Turn start, add poison (scales with item level).
/// On attack, deal a multiplier of the poison stacks on the enemy. (doesn't lower stacks)
/// </summary>
public class PlagueExplosion : BaseItem, IOnAttackItem, IUpgradableItem
{
    private int additionalPoison = 1;
    private float damageMultiplier = 3;
    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; } = 0.75f;
    public void OnTurnStart()
    {
        foreach (BaseStatus status in EnemyCharacter.Instance.statusEffects)
        {
            if (status is Poison poison)
            {
                poison.IncreaseStack(additionalPoison);
            }
        }
    }
    public void OnAttack(EnemyCharacter character, int damage)
    {
            Poison poison = EnemyCharacter.Instance.statusEffects.OfType<Poison>().FirstOrDefault();
            if (poison != null)
            {
                EnemyCharacter.Instance.LoseMana(Mathf.RoundToInt(poison.StackCount * damageMultiplier));
            }
    }
    public void Upgrade()
    {
        Tier++;
        damageMultiplier += UpgradeModifier;
        additionalPoison += UpgradeAmount;
    }
    public override string GetEffectDesc()
    {
        return $"When attacking an enemy affected by poison, deal {damageMultiplier * 100}% of poison stacks as additional damage." +
            $"\nThe plague spreads at the start of each turn, adding {additionalPoison} stacks to the enemy.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}" +
            $"\nEach tier increases damage multiplier by {UpgradeModifier} and poison stack increases by {UpgradeAmount}.";
    }
}
