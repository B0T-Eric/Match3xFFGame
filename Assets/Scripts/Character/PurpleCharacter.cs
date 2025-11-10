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
        description = $"<b><color=#{hexColour}> {manaType} Character </b></color>\n Gain potential via matching the {manaType} spirit tokens. " +
        $"\n <b>Passives: \n</b> Passive 1 (0 mana): Reduces damage taken by 5 and reduces damage taken by allies by 1." +
        $"\n Passive 2 ({passiveValues.secPassiveThreshold} mana): Heal back {RegenHealth} after taking damage." +
        $"\n Passive 3 ({passiveValues.thirdPassiveThreshold} mana): When taking damage deal {ReturnDamage} damage to the enemy.";

    }
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
