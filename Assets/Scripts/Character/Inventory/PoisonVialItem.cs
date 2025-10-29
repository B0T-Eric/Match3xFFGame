using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonVialItem : DraggableItem, IOnTurnEnd
{
    Sprite poisonSprite;
    public void OnTurnEnd()
    {
        Poison p = new Poison(poisonSprite);
        EnemyCharacter.Instance.AddStatusEffect(p);
    }
}
