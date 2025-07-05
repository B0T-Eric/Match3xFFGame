using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class WhiteCharacter : BaseCharacter
{
    public override void GainPotential(int amount)
    {
        if(amount == 4)
        {
            amount += 3;
        }
        else if(amount > 4)
        {
            amount += 6;
        }
        else
        {
            potential += amount;
        }
        UpdateDisplays();
    }
    public override void Init()
    {
        manaType = ManaType.White;
        hexColour = "FFFFFF";
        base.Init();
    }
    public override void Attack(BaseCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            int damage = GetDamage();
            if (damage > 0)
            {
                c.LoseMana(damage);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c);
                }
                if (currentMana > secPassiveThreshold)
                {
                    c.LoseMana(damage);
                    //delegate for apply on hit effects/extra damage
                    if (onAttack != null)
                    {
                        onAttack.Invoke(c);
                    }

                }
            }
        }
        nextTurnPotential = 0;
    }
}
