using Assets.Scripts.Character.CharacterAttacks;
using System.Linq;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class WhiteCharacter : BaseCharacter
{
    float ItemPotentialModifier = 0.15f;
    [SerializeField]
    TwoHitAttack doubleAttack;
    [SerializeField]
    PreparationAttack preparationAttack;
    [SerializeField]
    QuickAttack quickAttack;
    int bUseCount = 2;
    float bModifier = 0.5f;
    int weakDuration = 2;
    float weakModifier = 0.25f;
    [SerializeField]
    Sprite brittleSprite, weakSprite;
    
    public override void GainPotential(int amount)
    {
        if(amount == 4)
        {
            amount += 3;
        }
        if(amount > 4)
        {
            amount += 6;
        }
        amount += GetPotentialIncrease();
        amount = (int)Mathf.Round(amount * GetPotentialMult());
        potential += amount;
        UpdateDisplays();
    }
    protected override void UpdatePassives()
    {
        if (preparationAttack.IsPreparing)
        {
            turnStateButton.interactable = false;
        }
        else
        {
            turnStateButton.interactable = true;
        }
        if (IsPassiveTwoActive)
        {
            if (!attacks.Contains(preparationAttack))
            {
                attacks.Add(preparationAttack);
            }
            if (!attacks.Contains(doubleAttack))
            {
                attacks.Add(doubleAttack);
            }
            if (!attacks.Contains(quickAttack))
            {
                attacks.Add(quickAttack);
            }

        }
        else
        {
            if (attacks.Contains(preparationAttack))
            {
                attacks.Remove(preparationAttack);
            }
            if (attacks.Contains(doubleAttack))
            {
                attacks.Remove(doubleAttack);
            }
            if (attacks.Contains(quickAttack))
            {
                attacks.Remove(quickAttack);
            }
            selectedAttack = 0;
        }
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Matches of spirit tokens 4+ gain more potential: 4 gets an additional 3 potential, 5+ gets an additional 6.\n" +
            $"Each equiped item on this character increases potential retained by {ItemPotentialModifier * 100}%";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Gains 3 new attacks:\n" +
            $"<b>Preparation:</b> {GetCharacterName()} prepares for their next strike across 3 turns, dealing massive (300% damage) amounts of damage once preparation is complete.\n" +
            $"<b>Twin Strike:</b> {GetCharacterName()} strikes an enemy 2 times for 100% damage on each hit.\n" +
            $"<b>Quick Strike:</b> {GetCharacterName()} attack for 75% damage and gain 5 potentail next turn.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"When attacking, apply weakness to enemies above 50% max health or brittle to enemies below.\n" +
            $"Brittle increases damage taken by {bModifier * 100}% for the next {bUseCount} attacks.\n" +
            $"Weakness decreases damage dealt by {weakModifier * 100}% for {weakDuration} turns.";
    }
    #endregion
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility(EnemyCharacter.Instance);
    }
    protected override float GetRetainMult()
    {
        float mult = 0.75f;
        foreach(DraggableItem item in items)
        {
            mult += ItemPotentialModifier;
        }
        return mult;
    }
    public void OnAttack(BaseCharacter character, int damage)
    {
        Brittle brittle = new Brittle(bUseCount, bModifier, brittleSprite);
        Weakness weakness = new Weakness(weakDuration,weakModifier,weakSprite);
        int halfHp = character.maxMana / 2;
        if (character.currentMana > halfHp)
        {
            character.AddStatusEffect(weakness);
        }
        else
        {
            character.AddStatusEffect(brittle);
        }
    }
    public override void Init()
    {
        manaType = ManaType.White;
        hexColour = "FFFFFF";
        base.Init();

    }
}
