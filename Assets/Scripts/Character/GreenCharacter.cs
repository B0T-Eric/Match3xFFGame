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
    public override void GainPotential(int amount)
    {
        if (currentMana > passiveValues.secPassiveThreshold)
        {
            amount = Mathf.RoundToInt(amount * 0.66f);
        }
        else if (currentMana < passiveValues.thirdPassiveThreshold)
        {
            amount = Mathf.RoundToInt(amount * 1.33f);
        }
        amount = Mathf.RoundToInt(amount * 0.33f);
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
