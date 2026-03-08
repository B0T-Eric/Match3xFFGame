using UnityEngine;
/// <summary>
/// 
/// </summary>
public class GreenCharacter : BaseCharacter
{
    public static float revengeDamage = 3f;
    public static int baseRevengeDamage = 20;
    [SerializeField]
    private GreenSpecialAttack spec;
    public static bool hasSpec = false;
    public static bool usedSpec = false;
    public override void Init()
    {
        manaType = ManaType.Green;
        hexColour = "1CFF00";
        onDamageTaken += DamageTaken;
        base.Init();
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Potential gained from matches is decreased by 33% (67% of base) but when healing heal all party members. (this character gains increased mana from healing actions)";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Gain a new attack once per combat upon being hit by the enemy.\n" +
            $"<color=#1CFF00><b>Natures Revenge:</b></color> Empowered attack that deals {baseRevengeDamage} plus standard attack damage increased by an additional {revengeDamage * 100}%.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Potential gain is doubled. (133% of base)";
    }
    #endregion
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility();
    }
    protected override void UpdatePassives()
    {
        if (usedSpec)
        {
            attacks.Remove(spec);
            selectedAttack = 0;
        }
    }
    public void DamageTaken(BaseCharacter character, int dmg)
    {
        if(!hasSpec && !usedSpec && IsPassiveTwoActive)
        {
            hasSpec = true;
            attacks.Add(spec);
        }
    }
    public override void GainPotential(int amount)
    {
        if (currentMana >= passiveValues.thirdPassiveThreshold)
        {
            amount = Mathf.RoundToInt(amount * 1.33f);
        }
        else
        {
            amount = Mathf.RoundToInt(amount * 0.66f);
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
        amount += Mathf.RoundToInt(amount * 1.33f);
        base.HealMana(amount);
        
    }
}
