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
    public void OnAttack(BaseCharacter character, int damage)
    {
        int rng = Random.Range(0, 2);
        if (rng == 0)
        {
            Poison p = new Poison(poisonSprite);
            character.AddStatusEffect(p);
        }
        
    }
}
