using System.Collections;
using UnityEngine;

public class CursedFlameShield : BaseItem, IOnDamageTakenItem, IUpgradableItem
{
    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; }
    [SerializeField]
    Sprite burnSprite;
    private int burnDamage = 3;
    private int duration = 2;
    
    public void Upgrade()
    {
        Tier++;
        burnDamage += UpgradeAmount;
    }
    public void OnDamageTaken(BaseCharacter character, int dmg)
    {
        Burn b = new Burn(duration, burnDamage, EnemyCharacter.Instance, burnSprite);
        StatusHandler.AddStatusEffect(b, EnemyCharacter.Instance);
    }
    public override string GetEffectDesc()
    {
        return $"On damage taken, apply Burn to the enemy, dealing {burnDamage} each turn for {duration} turns.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}." +
            $"\nEach tier provides {UpgradeAmount} additional burn damage.";
    }
    
}