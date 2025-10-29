using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orange character class facilitates the unique ablities and evolutions of the orange character type.
/// The primary unique effect of Orange Character is to apply burn status effects to amplify their damage output.
/// Burn is not unique to Orange character - can be applied bye Items or Enemies.
/// Passive level 1: apply half damage burn for 2 turns (total 100% damage of hit).
/// Passive level 2: apply half + 5 damage burn for 2 turns (total 100% + 10 damage of hit).
/// Passive level 3: tbd
/// </summary>
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
            //animate
            int damage = GetDamage();
            int burnDamage = 0;
            if(currentMana > passiveValues.secPassiveThreshold)
            {
                burnDamage = (damage / 2) + 5;
            }
            else
            {
                burnDamage = damage / 2;
            }
            if(burnDamage > 0 && damage > 0)
            {
                Burn b = new Burn(2, burnDamage, c, Icon);
                c.AddStatusEffect(b);
                if (onAttack != null)
                {
                    onAttack.Invoke(c, damage);
                }
                c.LoseMana(damage, this.GetType().Name);
            }
        }
        nextTurnPotential = 0;
    }
}
