using System.Collections;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class YellowCharacter : BaseCharacter
{
    //heal amount 
    int heal = 5;
    public Sprite weaknessIcon;
    int weaknessDuration = 2;
    float weaknessEffect = 0.75f;
    public override void Init()
    {
        manaType = ManaType.Yellow;
        hexColour = "FFEA00";
        base.Init();

    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Provides the ability to see how much damage the enemy is doing each turn and who they are attacking.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Applies weakness on attack. \n" +
        $"<color=#{hexColour}>Weakness:</color> Reduces outgoing damage by {weaknessEffect * 100}% for {weaknessDuration} turns.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"At the beginning of each turn, restore {heal} mana to all party memebers";
    }
    #endregion
    public override void Attack(BaseCharacter c)
    {
        
        if (currentMana > passiveValues.secPassiveThreshold)
        {
            ApplyWeakness(c);
        }
        base.Attack(c);

    }
    private void ApplyWeakness(BaseCharacter c) 
    {
        //Modifier = 0.75 (25% reduction), Duration = 2 turns.
        Weakness weakness = new Weakness(weaknessDuration, weaknessEffect, weaknessIcon);
        c.AddStatusEffect(weakness);
    }
    public override void TurnStart()
    {
        if (currentMana > passiveValues.thirdPassiveThreshold)
        {
            foreach (BaseCharacter c in Party.Instance.members)
            {
                c.HealMana(heal);
            }
        }
        base.TurnStart();
    }
}

