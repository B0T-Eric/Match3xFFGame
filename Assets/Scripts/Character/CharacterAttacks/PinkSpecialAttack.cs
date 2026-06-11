using System.Collections;
using UnityEngine;

public class PinkSpecialAttack : BasicAttack
{
    public override void Attack(EnemyCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackEnemyDelegate onAttack)
    {
        if(damage < 10 && origin.IsPassiveThreeActive)
        {
            damage = 10;
        }
        int mult = GetMult(damage,origin);
        if (origin.IsPassiveTwoActive)
        {
            for(int i = 0; i < mult; i++)
            {
                Hit(c, damage,origin, characterName, onAttack);
            }
        }
        else
        {
            int increasedDamage = damage * mult;
            Hit(c, increasedDamage, origin, characterName, onAttack);
        }
    }
    int GetMult(int damage,BaseCharacter character)
    {
        int count = 0;
        if (damage % 6 == 0)
            count++;

        if (damage % 4 == 0)
            count++;

        if (damage % 3 == 0)
            count++;
        if(character.IsPassiveTwoActive && damage % 5 == 0 && count < 3)
        {
            count++;
        }

        if (count == 0)
            count = 1;
        return count;
    }
}
