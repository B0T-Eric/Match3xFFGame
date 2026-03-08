using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Character.CharacterAttacks
{
    public class QuickAttack : BasicAttack
    {
        public override void Attack(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
        {
            origin.nextTurnPotential = Mathf.RoundToInt(damage * 0.25f);
            damage = Mathf.RoundToInt(damage * 0.75f);
            
            base.Attack(c, damage, origin, characterName, onAttack);
        }
    }
}