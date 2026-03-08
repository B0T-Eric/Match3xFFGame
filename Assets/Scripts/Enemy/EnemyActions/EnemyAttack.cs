using System.Collections;
using UnityEngine;


public class EnemyAttack : EnemyAction
{
    public override void PreformAction(BaseCharacter target)
    {
        target.display.AttackTrue();
    }
}
