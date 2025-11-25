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
    private int stacksToApply = 3;
    public void OnAttack(BaseCharacter character, int damage)
    {
        if(itemHolder.statusEffects.Count > 0)
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
        return $"Exploit enemies who are already suffering from an ailment (status effect).\n" +
            $"Apply {stacksToApply} stacks of poison on attack.";
    }
}
