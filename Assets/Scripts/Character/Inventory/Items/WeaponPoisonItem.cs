using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This item will randomly (coinflip - 50% chance) apply poison on attack from item holder
/// </summary>
public class WeaponPoisonItem : DraggableItem, IOnAttackItem
{
    [SerializeField]
    Sprite poisonSprite;
    int stacksToApply = 1;
    public void OnAttack(BaseCharacter character, int damage)
    {
        int rng = Random.Range(0, 2);
        if (rng == 0)
        {
            Poison p = new Poison(poisonSprite);
            for (int i = 0; i < stacksToApply; i++)
            {
                character.AddStatusEffect(p);
            }
        }
        
    }
    public override string GetEffectDesc()
    {
        return $"On attack there is a 50% chance to apply {stacksToApply} stacks of poison to the enemy.";
    }
}
