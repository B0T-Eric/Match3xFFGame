using System.Collections;
using UnityEngine;

public class FireStormAttack : BasicAttack
{
    public override void Attack(BaseCharacter target, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack, BaseStatus status)
    {
        int burnCount = damage / 7;
        int burnDamage = damage / 2;
        if(status is SuperBurn sb)
        {
            sb.Damage = burnDamage;
            for (int i = 0; i < burnCount; i++)
            {
                ApplySuperBurn(sb);
            }
        }
        else if(status is Burn b)
        {
            b.Damage = burnDamage;
            for (int i = 0; i < burnCount; i++)
            {
                ApplyBurn(b);
            }
        }

    }
    void ApplyBurn(Burn burn)
    {
        EnemyCharacter.Instance.AddStatusEffect(burn);
    }
    void ApplySuperBurn(SuperBurn burn)
    {
        EnemyCharacter.Instance.AddStatusEffect(burn);
    }
}