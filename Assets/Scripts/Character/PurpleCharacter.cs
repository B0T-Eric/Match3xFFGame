using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Passively gives allies damage reduction
/// regains 
/// </summary>
public class PurpleCharacter : BaseCharacter
{
    public int ReturnDamage;
    public int RegenHealth;
    public override void Init()
    {
        manaType = ManaType.Purple;
        hexColour = "AD00FF";
        base.Init();

    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Reduces damage taken by 5 and reduces damage taken by allies by 1.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Heal back {RegenHealth} after taking damage.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"When taking damage deal {ReturnDamage} damage to the enemy.";
    }
    #endregion
    public override void LoseMana(int amount, string source = "Unknown")
    {
        base.LoseMana(amount);
        if(currentMana > passiveValues.secPassiveThreshold)
        {
            HealMana(RegenHealth);
        }
        if(currentMana > passiveValues.thirdPassiveThreshold)
        {
            EnemyCharacter.Instance.LoseMana(ReturnDamage);
        }
    }
}
