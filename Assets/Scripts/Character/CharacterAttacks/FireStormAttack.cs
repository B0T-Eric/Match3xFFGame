using System.Collections;
using UnityEngine;

public class FireStormAttack : BasicAttack
{
    public override void Attack(EnemyCharacter target, int damage, BaseCharacter origin, string characterName, OnAttackEnemyDelegate onAttack, BaseStatus status)
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
        StatusHandler.AddStatusEffect(burn, EnemyCharacter.Instance);
    }
    void ApplySuperBurn(SuperBurn burn)
    {
        StatusHandler.AddStatusEffect(burn, EnemyCharacter.Instance);
    }
}