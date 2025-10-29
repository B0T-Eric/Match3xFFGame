using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This item applies multiple stacks of poison on attack 
/// if they are currently buffed or debuffed
/// </summary>
public class PoisonChaliceItem : DraggableItem, IOnAttackItem
{
    [SerializeField]
    Sprite poisonSprite;
    public void OnAttack(BaseCharacter character, int damage)
    {
        if(itemHolder.statusEffects.Count > 0)
        {
            Poison p = new Poison(poisonSprite);
            character.AddStatusEffect(p);
            character.AddStatusEffect(p);
            character.AddStatusEffect(p);
        }
    }
}
