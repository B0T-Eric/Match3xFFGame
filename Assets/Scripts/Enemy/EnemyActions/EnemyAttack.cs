using System.Collections;
using UnityEngine;


public class EnemyAttack : EnemyAction
{
    public override void PreformAction(EnemyCharacter enemyCharacter)
    {
        enemyCharacter.display.AttackTrue();
    }
}
