using System.Collections;
using UnityEngine;

public class GreenSpecialAttack : BasicAttack
{
    public override void Attack(EnemyCharacter c, int damage,BaseCharacter origin, string characterName, OnAttackEnemyDelegate onAttack)
    {
        damage += GreenCharacter.baseRevengeDamage;
        damage *= (int)GreenCharacter.revengeDamage;
        Hit(c, damage,origin, characterName, onAttack);
        GreenCharacter.usedSpec = true;
    }
}