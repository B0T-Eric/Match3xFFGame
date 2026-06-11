using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class YellowCharacter : BaseCharacter
{
    float baseEvasionChance = 0.25f;
    float evasionIncrease = 0.025f;
    float maxHpDamage = 0.03f;
    public override void Init()
    {
        stats = new YellowStats(statsSO.maxMana, statsSO.initialMana, statsSO.passiveTwo, statsSO.passiveThree, 0.75f);
        manaType = ManaType.Yellow;
        hexColour = "FFEA00";
        base.Init();

    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Provides the ability to see how much damage the enemy is doing each turn and who they are attacking.\nCan also see through enemy deception.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"While above {stats.pTwo} mana, gain a {baseEvasionChance*100}% chance to evade enemy damage.\n" +
            $"Evasion chance increases by {evasionIncrease * 100}% for each mana above 20.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"When evading retaliate with a swift attack dealing {maxHpDamage * 100}% of the enemies health.";
    }
    #endregion
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility(EnemyCharacter.Instance);
    }
    public override void LoseMana(int amount, string source = "Unknown")
    {
        float evadeChance = GetEvasionChance();
        if(Random.value < evadeChance)
        {
            if (IsPassiveThreeActive) //retaliate
            {
                Retaliation(amount);
            }
            else //ignore incoming damage.
            {
                return;
            }
        }
        else
        {
            base.LoseMana(amount, source);
        } 
    }
    void Retaliation(int dmg)
    {
        EnemyCharacter c = EnemyCharacter.Instance;
        int damage = Mathf.RoundToInt(c.stats.MaxMana * maxHpDamage);
        c.LoseMana(damage, $"{GetCharacterName()} retaliation");
        nextTurnPotential = damage;
    }
    private float GetEvasionChance()
    {
        float evasion = baseEvasionChance;
        float increase = 0;
        for (int i = 20; i < stats.CurrentMana; i++)
        {
            increase += evasionIncrease;
        }
        evasion += increase;
        return evasion;
    }
}

