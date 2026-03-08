using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class defined the poison ivy item which should apply a poison status effect 
/// to the enemy when the character with the item equipped is attacked by anything.
/// </summary>
public class PoisonIvyItem : DraggableItem, IOnDamageTakenItem, IUpgradableItem
{
    [SerializeField]
    Sprite poisonSprite;
    
    private Poison poison;
    private int stacksToApply = 1;
    public int Tier { get; set; }
    public int UpgradeAmount { get; set; } = 1;
    public float UpgradeModifier { get; set; }
    public void OnDamageTaken(BaseCharacter character,int dmg)
    {
        poison = new Poison(poisonSprite);
        for (int i = 0; i < stacksToApply; i++)
        {
            EnemyCharacter.Instance.AddStatusEffect(poison);
        }
    }
    public override string GetEffectDesc()
    {
        return $"Whenever the character who has this item equiped takes damage, apply {stacksToApply} stacks of poison to the enemy.";
    }
    public override string GetUpgradeEffectDesc()
    {
        return $"Current Tier: {Tier}" +
            $"\nIncreases amound of stacks applied by {UpgradeAmount} per tier.";
    }
    public void Upgrade()
    {
        Tier++;
        stacksToApply += UpgradeAmount;
    }
}
