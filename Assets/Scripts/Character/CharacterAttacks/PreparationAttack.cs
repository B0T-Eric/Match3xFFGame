using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Character.CharacterAttacks
{
    public class PreparationAttack : BasicAttack
    {
        public bool IsPreparing = false;
        int timer = 0;
        int timerStop = 3;
        [SerializeField]
        float multiplier;
        public override void Attack(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
        {
            if(timer == timerStop)
            {
                //do big attack
                damage = Mathf.RoundToInt(damage * multiplier);
                Hit(c, damage, origin, characterName,onAttack);
                timer = 0;
                IsPreparing = false;
            }
            else
            {
                if(timer == 0)
                {
                    IsPreparing = true;
                }
                timer++;
            }
        }
    }
}