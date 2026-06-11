using System.Collections;
using UnityEngine;

public class TwoHitAttack : BasicAttack
{
    public override void Attack(EnemyCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackEnemyDelegate onAttack)
    {
        Hit(c, damage, origin, characterName, onAttack);    
        Hit(c, damage, origin, characterName, onAttack);
        
    }
}
