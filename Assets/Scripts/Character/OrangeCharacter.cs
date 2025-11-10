using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orange character class facilitates the unique ablities and evolutions of the orange character type.
/// The primary unique effect of Orange Character is to apply burn status effects to amplify their damage output.
/// Burn is not unique to Orange character - can be applied bye Items or Enemies.
/// Passive level 1: apply half damage burn for 2 turns (total 100% damage of hit).
/// Passive level 2: apply half + 5 damage burn for 2 turns (total 100% + 10 damage of hit).
/// Passive level 3: 
/// </summary>
public class OrangeCharacter : BaseCharacter
{
    public Sprite Icon;
    float burnDamagePercent = 0.5f;
    int burnDuration = 2;
    int burnBonusDamage = 5;
    int storedDamage = 0;
    float storedDamagePercent = 0.25f;
    public override void Init()
    {
        manaType = ManaType.Orange;
        hexColour = "FF8000";
        base.Init();
        description = $"<b><color=#{hexColour}> {manaType} Character </b></color>\n Gain potential via matching the {manaType} spirit tokens. " +
        $"\n <b>Passives: \n</b> Passive 1 (0 mana): Attacks set enemies on fire, applying the burn status effect for {burnDamagePercent * 100}% damage." +
        $"\n <color=#{hexColour}>Burn:</color> Deals damage at the end of turn for {burnDuration} turns." +
        $"\n Passive 2 ({passiveValues.secPassiveThreshold} mana): Adds {burnBonusDamage} to new burn status effects." +
        $"\n Passive 3 ({passiveValues.thirdPassiveThreshold} mana): Stores {storedDamagePercent * 100}% damage from the previous round to increase damage in the next";

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
                burnDamage = Mathf.RoundToInt((damage * burnDamagePercent) + burnBonusDamage);
            }
            else
            {
                burnDamage = Mathf.RoundToInt(damage*burnDamagePercent);
            }
            if(burnDamage > 0 && damage > 0)
            {
                Burn b = new Burn(burnDuration, burnDamage, c, Icon);
                c.AddStatusEffect(b);
                if (onAttack != null)
                {
                    onAttack.Invoke(c, damage);
                }
                c.LoseMana(damage, this.GetType().Name);
                if(currentMana > passiveValues.thirdPassiveThreshold)
                {
                    storedDamage = Mathf.RoundToInt(damage * storedDamagePercent);
                }
            }
        }
        nextTurnPotential = 0;
    }
    protected override int GetDamage()
    {
        int damage = potential;
        if(currentMana > passiveValues.thirdPassiveThreshold)
        {
            damage += storedDamage;
        }
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.flatDamageIncrease > 0)
            {
                damage += item.itemSO.flatDamageIncrease;
            }
            if (item.itemSO.damageMult > 0)
            {
                damage = (int)(damage * item.itemSO.damageMult);
            }
        }
        return damage;
    }
}
