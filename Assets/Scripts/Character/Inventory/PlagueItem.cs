using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlagueItem : DraggableItem, IOnTurnEnd, IOnTurnStart
{
    public void OnTurnEnd()
    {
        Poison poison = EnemyCharacter.Instance?.statusEffects?.OfType<Poison>().FirstOrDefault();
        if(poison != null )
        {
            EnemyCharacter.Instance.LoseMana(poison.StackCount);
        }
    }

    public void OnTurnStart()
    {
        foreach (BaseStatus status in EnemyCharacter.Instance.statusEffects) 
        {
            if (status is Poison poison)
            {
                poison.DecreaseStack();
            }
        }
    }
}
