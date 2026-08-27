using System.Collections;
using UnityEngine;

public class EnemyHeal : EnemyAction
{
    public int HealAmount;
    public override void PreformAction(EnemyCharacter target)
    {
        target.Heal(HealAmount);
    }
}
