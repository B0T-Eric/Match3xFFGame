using UnityEngine;
/// <summary>
/// 
/// </summary>
public class GreenCharacter : BaseCharacter
{
    public override void Init()
    {
        manaType = ManaType.Green;
        hexColour = "1CFF00";
        base.Init();
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Potential gained from matches is decreased by 67% (33% of default) but when healing heal all party members.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Potential gain increases by 100% (66% of default).";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Potential gain increases by another 100% (133% of default).";
    }
    #endregion
    public override void GainPotential(int amount)
    {
        if (currentMana >= passiveValues.secPassiveThreshold)
        {
            amount = Mathf.RoundToInt(amount * 0.66f);
        }
        else if (currentMana >= passiveValues.thirdPassiveThreshold)
        {
            amount = Mathf.RoundToInt(amount * 1.33f);
        }
        else
        {
            amount = Mathf.RoundToInt(amount * 0.33f);
        }
        base.GainPotential(amount);
    }
    public override void HealMana(int amount)
    {
        foreach (BaseCharacter c in Party.Instance.members)
        {
            if (c != this)
            {
                c.HealMana(amount);
            }
        }
        base.HealMana(amount);
        
    }
}
