using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeCharacter : BaseCharacter
{
    public Sprite Icon;
    public override void Init()
    {
        manaType = ManaType.Orange;
        hexColour = "FF8000";
        base.Init();
        
    }

    //on attack from orange character, take half the damage delt, and the enemy and apply a burn effect.
    //each burn is independant
    public override void Attack(BaseCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            int damage = GetDamage();
            int burnDamage = 0;
            if(currentMana > secPassiveThreshold)
            {
                burnDamage = (damage / 2) + 5;
            }
            else
            {
                burnDamage = damage / 2;
            }
            Burn b = new Burn(2, burnDamage, c, Icon);
            c.AddStatusEffect(b);
            c.LoseMana(damage);
        }
        nextTurnPotential = 0;
    }
}
