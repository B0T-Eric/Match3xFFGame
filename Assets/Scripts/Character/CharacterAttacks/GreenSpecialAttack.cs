using System.Collections;
using UnityEngine;

public class GreenSpecialAttack : BasicAttack
{
    public override void Attack(BaseCharacter c, int damage,BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
    {
        damage += GreenCharacter.baseRevengeDamage;
        damage *= (int)GreenCharacter.revengeDamage;
        Hit(c, damage,origin, characterName, onAttack);
        GreenCharacter.usedSpec = true;
    }
}