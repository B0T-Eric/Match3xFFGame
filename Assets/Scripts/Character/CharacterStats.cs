
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


/// <summary>
/// Purpose of this class:
/// Character stats should handle the processing of stat values in the game through various methods.
/// Public Methods:
/// GetDamage -> Returns the damage for the character based on parsed in information, like items/delegates/modifiers etc.
/// GetResistance -> Returns the damage reduction applied when taking damage from attacks and some status effects.
/// GetHealing -> Returns the amount of Spirit Mana restored when healing.
/// Public Members:
/// 
/// </summary>
public class CharacterStats
{
    /// <summary>
    /// Default Empty Constructor.
    /// </summary>
    public CharacterStats()
    {

    }
    /// <summary>
    /// Constructor for initialising instance of the class, passed in the values from the base character class on Init.
    /// </summary>
    /// <param name="maxMana">int value representing maximum mana</param>
    /// <param name="currentMana"> int value representing the starting mana</param>
    public CharacterStats(int maxMana, int startMana)
    {
        MaxMana = maxMana;
        CurrentMana = startMana;
        Items = new();
    }

    /// <summary>
    /// Maximum spirit mana possible on a character.
    /// </summary>
    public int MaxMana { get; set; }
    /// <summary>
    /// Characters current spirit mana.
    /// </summary>
    public int CurrentMana { get; set; }
    /// <summary>
    /// int member which serves as a counter for tracking damage taken by the character each combat.
    /// </summary>
    public int damageTaken = 0;

    /// <summary>
    /// List of BaseItems contained by a character which can contain modifiers to damage/healing/resistance etc.
    /// </summary>
    public List<BaseItem> Items { get; set; }
    /// <summary>
    /// Delegate member which contains methods adding flat damage to outgoing damage.
    /// </summary>
    public OnGetDamage onGetDmgFlat;
    /// <summary>
    /// Delegate member which contains methods multiplying outgoing damage.
    /// </summary>
    public OnGetDamageMult onGetDmgMult;
    /// <summary>
    /// Delegate member which contains methods for adding or subtracting character healing amount.
    /// </summary>
    public OnGetHealFlat onGetHealFlat;
    /// <summary>
    /// Delegate member which contains methods for multiplying character healing.
    /// </summary>
    public OnGetHealMult onGetHealMult;
    /// <summary>
    /// Delegate member which contains methods for increasing or decreasing character damage reduction (integer value).
    /// </summary>
    public OnGetDamageReduction onGetResist;
    /// <summary>
    /// Default GetDamage method for retrieving base damage of a character.
    /// </summary>
    /// <returns>INT -> Base damage of the character</returns>
    public virtual int GetDamage()
    {
        return 0;
    }
    /// <summary>
    /// Overload Method for characters which are attacking, pulling a base value and then applying modifiers to return the out going damage value.
    /// </summary>
    /// <param name="baseDamage">potential or parsed in base damage</param>
    /// <returns>INT -> outgoing attack damage.</returns>
    public virtual int GetDamage(int baseDamage)
    {
        return 0;
    }
    /// <summary>
    /// Default GetHealing method for retrieving base healing of a character.
    /// </summary>
    /// <returns>base healing value of character</returns>
    public virtual int GetHealing()
    {
        return 0;
    }
    /// <summary>
    /// Overload 
    /// </summary>
    /// <param name="baseHeal"></param>
    /// <returns></returns>
    public virtual int GetHealing(int baseHeal)
    {
        return 0;
    }
    public virtual int GetResist()
    {
        return 0;
    }
    public virtual int GetResist(int baseResist)
    {
        return 0;
    }
}
public class EnemyCharacterStats : CharacterStats
{
    public int MinDamage;
    public int MaxDamage;
    public int Damage;
    /// <summary>
    /// Empty GetDamage call returns random damage using min and max damage.
    /// </summary>
    /// <returns>random int within range</returns>
    public override int GetDamage()
    {
        //initial damage roll
        int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
        if (onGetDmgFlat != null)
        {
            foreach (OnGetDamage getDamage in onGetDmgFlat.GetInvocationList().Cast<OnGetDamage>())
            {
                damage += getDamage.Invoke();
            }
        }
        int bonusDamage = 0;
        if (onGetDmgMult != null)
        {
            foreach (OnGetDamageMult getDamageMult in onGetDmgMult.GetInvocationList().Cast<OnGetDamageMult>())
            {
                bonusDamage += getDamageMult.Invoke(damage);
            }
        }
        return damage;
    }
}
public class WolfStats : EnemyCharacterStats
{
    public override int GetDamage()
    {
        return base.GetDamage();
    }
}
public class PlayerCharacterStats : CharacterStats
{
    /// <summary>
    /// Second Passive trigger threshold.
    /// </summary>
    public int pTwo;
    /// <summary>
    /// Third Passive trigger threshold.
    /// </summary>
    public int pThree;
    /// <summary>
    /// retain multiplier for the defensive / retain turn state.
    /// </summary>
    public float retainMult; //base value.

    public PlayerCharacterStats()
    {
    }

    public PlayerCharacterStats(int maxMana, int startMana, int p2, int p3, float rMult) : base(maxMana, startMana)
    {
        this.pTwo = p2;
        this.pThree = p3;
        this.retainMult = rMult;
    }
    public bool IsPassiveTwoActive => CurrentMana >= pTwo;
    public bool IsPassiveThreeActive => CurrentMana >= pThree;
    /// <summary>
    /// This override of the method is for player characters who are attacking - which means they're using potential when calculating damage output.
    /// </summary>
    /// <param name="potential">The characters current potential</param>
    /// <param name="items">The characters inventory of items needs to be parsed to process any stat increases on them.</param>
    /// <returns>modifier outgoing damage.</returns>
    public override int GetDamage(int potential)
    {
        int damage = potential;
        foreach (BaseItem item in Items)
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
        if (onGetDmgFlat != null)
        {
            foreach (OnGetDamage getDamage in onGetDmgFlat.GetInvocationList().Cast<OnGetDamage>())
            {
                damage += getDamage.Invoke();
            }
        }
        int baseDamage = damage;
        int bonusDamage = 0;
        if (onGetDmgMult != null)
        {
            foreach (OnGetDamageMult getDamageMult in onGetDmgMult.GetInvocationList().Cast<OnGetDamageMult>())
            {
                bonusDamage += getDamageMult.Invoke(baseDamage);
            }
        }
        damage = bonusDamage + baseDamage;
        return damage;
    }
    /// <summary>
    /// This override of the method is for players in round healing, which can be used for both the action and other healing actions.
    /// Adding various modifiers from items and on heal effects.
    /// </summary>
    /// <param name="potential">integer value indicating base heal amount</param>
    /// <returns>modified heal amount post item and other effects</returns>
    public override int GetHealing(int potential)
    {
        int healAmount = potential;
        int healAdd = 0;
        foreach(BaseItem item in Items)
        {
            if(item.itemSO.healFlat != 0)
            {
                healAdd += item.itemSO.healFlat;
            }
        }
        if(onGetHealFlat != null)
        {
            foreach(OnGetHealFlat flat in onGetHealFlat.GetInvocationList().Cast<OnGetHealFlat>())
            {
                healAdd += flat.Invoke();
            }
        }
        float healMult = 1;
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.healingMult != 0)
            {
                healMult += item.itemSO.healingMult;
            }
        }
        if (onGetHealMult != null)
        {
            foreach (OnGetHealMult mult in onGetHealMult.GetInvocationList().Cast<OnGetHealMult>())
            {
                healMult += mult.Invoke();
            }
        }
        int preMultHeal = healAmount += healAdd;
        healAmount = Mathf.RoundToInt(preMultHeal * healMult);
        return healAmount;
    }
    /// <summary>
    /// This override method is for player characters taking damage from attacks.
    /// </summary>
    /// <param name="baseResist">int variable which parsed from base character which is determined by turn state mostly</param>
    /// <returns>int value of flat damage reduction</returns>
    public override int GetResist(int baseResist)
    {
        int resist = baseResist;
        if (onGetResist != null)
        {
            foreach (OnGetDamageReduction reduction in onGetResist.GetInvocationList().Cast<OnGetDamageReduction>())
            {
                resist += reduction.Invoke();
            }
        }
        return resist;
    }
    /// <summary>
    /// This method takes the incoming potentail and applys any modifiers to return a new value.
    /// </summary>
    /// <param name="potential">int value coming from the match 3 game class</param>
    /// <returns>modified value of incoming potential increase.</returns>
    public virtual int GetPotential(int potential)
    {
        int basePotential = potential;
        int flat = 0;
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.potentialGainIncrease != 0)
            {
                flat += item.itemSO.potentialGainIncrease;
            }
        }
        float mult = 1f;
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.potentialGainMult != 0f)
            {
                mult += item.itemSO.potentialGainMult;
            }
        }
        int preMultPotential = basePotential += flat;
        basePotential = Mathf.RoundToInt(preMultPotential * mult);
        return basePotential;
    }
    /// <summary>
    /// This method takes in current turn potential and returns the modifier value for next turn potential
    /// </summary>
    /// <param name="potential">int value of current turn potential</param>
    /// <returns>int value of the next turn potential after modifiers</returns>
    public virtual int GetNextTurnPotential(int potential)
    {
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.retainMult != 0f)
            {
                retainMult += item.itemSO.retainMult;
            }
        }
        return Mathf.RoundToInt(potential * retainMult);
    }
}
public class WhiteStats : PlayerCharacterStats
{
    public float ItemPotentialModifier;

    public WhiteStats()
    {
    }

    public WhiteStats(int maxMana, int startMana, int p2, int p3, float rMult,float itemMod) : base(maxMana, startMana, p2, p3, rMult)
    {
        ItemPotentialModifier = itemMod;
    }

    public override int GetPotential(int potential)
    {
        int basePotential = potential;
        int flat = 0;
        if (potential == 4)
        {
            flat += 3;
        }
        if (potential > 4)
        {
            flat += 6;
        }
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.potentialGainIncrease != 0)
            {
                flat += item.itemSO.potentialGainIncrease;
            }
        }
        float mult = 1f;
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.potentialGainMult != 0f)
            {
                mult += item.itemSO.potentialGainMult;
            }
        }
        int preMultPotential = basePotential += flat;
        basePotential = Mathf.RoundToInt(preMultPotential * mult);
        return basePotential;
    }
    public override int GetNextTurnPotential(int potential)
    {
        float mult = retainMult;
        foreach (BaseItem item in Items)
        {
            mult += ItemPotentialModifier;
        }
        foreach (BaseItem item in Items)
        {
            if (item.itemSO.retainMult != 0f)
            {
                mult += item.itemSO.retainMult;
            }
        }
        return Mathf.RoundToInt(potential * mult);
    }
}
public class PinkStats : PlayerCharacterStats
{
    public PinkStats()
    {
    }

    public PinkStats(int maxMana, int startMana, int p2, int p3, float rMult) : base(maxMana, startMana, p2, p3, rMult)
    {
    }
}
public class BlueStats : PlayerCharacterStats
{
    private int FlatAmp;
    private float MultAmp;
    private float p3Amp;
    public BlueStats()
    {
    }

    public BlueStats(int maxMana, int startMana, int p2, int p3, float rMult, int flatAmp, float multAmp, float p3Amp) : base(maxMana, startMana, p2, p3, rMult)
    {
        FlatAmp = flatAmp;
        MultAmp = multAmp;
        this.p3Amp = p3Amp;
    }
    public override int GetDamage(int potential)
    {
        int damage = base.GetDamage();
        int debuffCount = EnemyCharacter.Instance.statusEffects.Where(status => status is IDebuff).Count();
        //increase damage by amount of debuffs on the character;
        if (debuffCount > 0)
        {
            int percentIncrease = Mathf.RoundToInt(damage * (MultAmp * debuffCount));
            int flatIncrease = damage + (debuffCount * FlatAmp);
            //when third passive is active increase the total damage gained from debuffs by 25%;
            if (IsPassiveThreeActive)
            {
                percentIncrease = Mathf.RoundToInt(percentIncrease * p3Amp);
                flatIncrease = Mathf.RoundToInt(flatIncrease * p3Amp);
            }
            //pick the higher value to increase damage by.
            if (flatIncrease > percentIncrease)
            {
                damage += flatIncrease;
            }
            else
            {
                damage += percentIncrease;
            }
        }
        return damage;
    }
}
public class YellowStats : PlayerCharacterStats
{
    public YellowStats()
    {
    }

    public YellowStats(int maxMana, int startMana, int p2, int p3, float rMult) : base(maxMana, startMana, p2, p3, rMult)
    {
    }
}
public class GreenStats : PlayerCharacterStats
{
    private float potentialModifier1;
    private float potentialModifier2;
    public GreenStats()
    {
    }

    public GreenStats(int maxMana, int startMana, int p2, int p3, float rMult, float pM1, float pM2) : base(maxMana, startMana, p2, p3, rMult)
    {
        potentialModifier1 = pM1;
        potentialModifier2 = pM2;
    }
    public override int GetPotential(int potential)
    {
        if (IsPassiveThreeActive)
        {
            potential = Mathf.RoundToInt(potential * potentialModifier2);
        }
        else
        {
            potential = Mathf.RoundToInt(potential * potentialModifier1);
        }
        return base.GetPotential(potential);
    }
}
public class OrangeStats : PlayerCharacterStats
{
    public OrangeStats()
    {
    }

    public OrangeStats(int maxMana, int startMana, int p2, int p3, float rMult) : base(maxMana, startMana, p2, p3, rMult)
    {
    }
}
public class PurpleStats : PlayerCharacterStats
{
    public PurpleStats()
    {
    }

    public PurpleStats(int maxMana, int startMana, int p2, int p3, float rMult) : base(maxMana, startMana, p2, p3, rMult)
    {
    }
}