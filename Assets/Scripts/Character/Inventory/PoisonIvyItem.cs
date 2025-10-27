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
    
    public void OnDamageTaken(BaseCharacter character)
    {
        poison = new Poison(poisonSprite);
        EnemyCharacter.Instance.AddStatusEffect(poison);
    }
}
