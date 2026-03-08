using System.Collections;
using UnityEngine;

public class EnemyHeal : EnemyAction
{
    public int HealAmount;
    public override void PreformAction(BaseCharacter target)
    {
        target.HealMana(HealAmount);
    }
}
