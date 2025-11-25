using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class defined the poison ivy item which should apply a poison status effect 
/// to the enemy when the character with the item equipped is attacked by anything.
/// </summary>
public class PoisonIvyItem : DraggableItem, IOnDamageTakenItem
{
    [SerializeField]
    Sprite poisonSprite;
    
    private Poison poison;
    private int stacksToApply = 1;
    public void OnDamageTaken(BaseCharacter character)
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
}
