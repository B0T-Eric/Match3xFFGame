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
        if(amount > 4)
        {
            amount += 6;
        }
        potential += amount;
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
            //animate
            int damage = GetDamage();
            if (damage > 0)
            {
                
                c.LoseMana(damage, this.GetType().Name);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c);
                }
                if (currentMana > passiveValues.secPassiveThreshold)
                {
                    
                    c.LoseMana(damage, this.GetType().Name);
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
