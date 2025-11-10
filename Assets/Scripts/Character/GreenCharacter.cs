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
        description = $"<b><color=#{hexColour}> {manaType} Character </b></color>\n Gain potential via matching the {manaType} spirit tokens. " +
            $"\n <b>Passives: \n</b> Passive 1 (0 mana): Potential gained from matches is decreased by 67% (33% of default) but when healing heal all party members." +
            $"\n Passive 2 ({passiveValues.secPassiveThreshold} mana): Potential gain increases by 100% (66% of default)." +
            $"\n Passive 3 ({passiveValues.thirdPassiveThreshold} mana): Potential gain increases by another 100% (133% of default).";

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
