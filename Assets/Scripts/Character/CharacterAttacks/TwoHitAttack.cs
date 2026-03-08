using System.Collections;
using UnityEngine;

public class TwoHitAttack : BasicAttack
{
    public override void Attack(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
    {
        Hit(c, damage, origin, characterName, onAttack);
        Hit(c, damage, origin, characterName, onAttack);
        
    }
}
