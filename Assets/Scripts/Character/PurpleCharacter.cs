using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
/// <summary>
/// Passively gives allies damage reduction
/// regains 
/// </summary>
public class PurpleCharacter : BaseCharacter
{
    private int AllyDamageResistances = 1;
    private int PurpleDamageResistance = 5;
    private int AllyDamageResistanceBuff = 2;
    private int PurpleDamageResistanceBuff = 3;
    private float retaliationMult = 0.5f;
    private float retaliationBuff = 0.25f;
    private float healOnHit = 0.25f;
    private int ReviveHealAmount = 15;
    private bool ReviveAvailable = false;
    public override void Init()
    {
        manaType = ManaType.Purple;
        hexColour = "AD00FF";
        base.Init();
        ApplyPassive();
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Reduces damage taken by {PurpleDamageResistance} and reduces damage taken by allies by {AllyDamageResistances}. These values increase with passive levels.\n" +
            $"Retaliate against enemies when they hit you for {retaliationMult * 100}% of the incoming pre-mitigated damage.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Increase the damage reduction given by {AllyDamageResistanceBuff} to allies and {PurpleDamageResistanceBuff} to this character.\n" +
            $"Additionally retaliation damage is increased by {retaliationBuff*100}% and heal for {healOnHit * 100}% of the damage taken.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Gain a 1 time revive (per combat), which revives this character when downed once, healing mana by {ReviveHealAmount}." +
            $"\nHealing recieved from the revive is shared to all allies by 75%." +
            $"\nRevive Available:{ReviveAvailable}.";
    }
    #endregion
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility(EnemyCharacter.Instance);
    }
    public override void LoseMana(int amount, string source = "Unknown")
    {
        if (state != CharacterState.Dead && currentMana > 0)
        {
            display.DefendTrue();
            amount -= GetDamageReduction();
            amount = Mathf.Max(amount, 0);
            IncreaseDamageTaken(amount);
            currentMana -= amount;
            if (onDamageTaken != null)
            {
                foreach (OnDamageTakenDelegate onDamageTaken in onDamageTaken.GetInvocationList().Cast<OnDamageTakenDelegate>())
                {
                    onDamageTaken.Invoke(this, amount);
                }
            }
            //if the character is down and has dropped below (or equal to) 0, set them to dead.
            if (currentMana <= 0)
            {
                if (state == CharacterState.Down)
                {
                    currentMana = 0;
                    display.SetState(CharacterState.Dead);
                }
                //if the character has dropped below (or equal to) 0 and is alive set them to down.
                else if (state == CharacterState.Alive)
                {
                    if (ReviveAvailable)
                    {
                        HealMana(ReviveHealAmount);
                        foreach(BaseCharacter character in Party.Instance.members)
                        {
                            HealMana(Mathf.RoundToInt(ReviveHealAmount * 0.75f));
                        }
                    }
                    else
                    {
                        currentMana = 1;
                        display.SetState(CharacterState.Down);
                    }
                }
            }
            UpdatePassives();
            CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} took {amount} damage from {source}");
        }
        UpdateDisplays();
    }
    protected override void UpdatePassives()
    {
        if (IsPassiveThreeActive && !ReviveAvailable)
        {
            ReviveAvailable = true;
        }
    }
    void ApplyPassive()
    {
        foreach(BaseCharacter c in Party.Instance.members)
        {
            c.onGetDamageReduction += AllyReduction;
        }
        onGetDamageReduction += SelfReduction;
    }
    int AllyReduction()
    {
        return IsPassiveTwoActive ? AllyDamageResistances + AllyDamageResistanceBuff : AllyDamageResistances;
    }
    int SelfReduction()
    {
        return IsPassiveTwoActive ? PurpleDamageResistance + PurpleDamageResistanceBuff : PurpleDamageResistance;
    }
    public void OnDamageTaken(BaseCharacter c, int damage)
    {
        int p1 = Mathf.RoundToInt(damage * retaliationMult);
        int p2 = Mathf.RoundToInt(damage * (retainMultiplier + retaliationBuff));
        if (IsPassiveTwoActive)
        {
            Retaliate(p2);    
            int heal = Mathf.RoundToInt(damage / healOnHit);
            HealMana(heal);
        }
        else
            Retaliate(p1);
    }
    public override void CombatStart()
    {
        if (IsPassiveThreeActive)
        {
            ReviveAvailable = true;
        }
    }
    void Retaliate(int damage)
    {
        EnemyCharacter.Instance.LoseMana(damage, GetCharacterName());
    }
}
