using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// when out going damage from the holders attacking turn, deal damage equal to stack count times a multiplier.
/// </summary>
public class PoisonCatalyst : DraggableItem, IOnAttackItem, IUpgradableItem, ICombinableItem
{
    private float damageMultiplier = 3;
    private int damageThreshold = 20;

    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; } = 0.5f;

    public void OnAttack(BaseCharacter character, int damage)
    {
        if(damage >= damageThreshold)
        {
            var poison = EnemyCharacter.Instance?.statusEffects?.OfType<Poison>().FirstOrDefault();
            if (poison != null)
            {
                EnemyCharacter.Instance.LoseMana(Mathf.RoundToInt(poison.StackCount * damageMultiplier));
            }
        }
    }
    
    public override string GetEffectDesc()
    {
        return $"When damage dealt by the character who currently has this item equiped exceeds {damageThreshold} and the enemy is poisoned,\n deal {damageMultiplier} x poison stacks as additional damage.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}" +
            $"\nUpgrading the item adds {UpgradeModifier * 100f}% extra multiplier to the damage." +
            $"\nAdditionally, decreases the threshold for the damage to trigger by {UpgradeAmount}.";
    }
    public void Upgrade()
    {
        Tier++;
        damageMultiplier += UpgradeModifier;
        damageThreshold -= UpgradeAmount;
    }
}
