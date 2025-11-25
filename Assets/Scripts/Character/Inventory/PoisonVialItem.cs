using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonVialItem : DraggableItem, IOnTurnEnd
{
    Sprite poisonSprite;
    int stacksToApply = 1;
    public void OnTurnEnd()
    {
        Poison p = new Poison(poisonSprite);
        for (int i = 0; i < stacksToApply; i++)
        {
            EnemyCharacter.Instance.AddStatusEffect(p);
        }
    }
    public override string GetEffectDesc()
    {
        return $"Apply a {stacksToApply} stack poison to the enemy at end of turn.";
    }
}
