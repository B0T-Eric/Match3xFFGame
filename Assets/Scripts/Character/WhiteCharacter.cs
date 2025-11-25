using UnityEngine;
/// <summary>
/// 
/// </summary>
public class WhiteCharacter : BaseCharacter
{
    float ItemPotentialModifier = 0.33f;
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
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Matches of spirit tokens higher than 3 gain more potential: 4 gets an additional 3 potential, 5+ gets an additional 6.Reduces damage taken by 5 and reduces damage taken by allies by 1.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Now attacks twice for full damage.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Each equiped item on this character increases potential gained by {ItemPotentialModifier * 100}%";
    }
    #endregion
    public override float GetPotentialMult()
    {
        float mult = 1f;
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.potentialGainMult != 0f)
            {
                mult += item.itemSO.potentialGainMult;
            }
            mult += ItemPotentialModifier;
        }
        return mult;
    }
    public override void Init()
    {
        manaType = ManaType.White;
        hexColour = "FFFFFF";
        base.Init();

    }
    public override void Attack(BaseCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            //animate
            int damage = GetDamage();
            if (damage > 0)
            {
                
                c.LoseMana(damage, this.GetType().Name);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c, damage);
                }
                if (currentMana > passiveValues.secPassiveThreshold)
                {
                    
                    c.LoseMana(damage, this.GetType().Name);
                    //delegate for apply on hit effects/extra damage
                    if (onAttack != null)
                    {
                        onAttack.Invoke(c, damage);
                    }

                }
            }
        }
        nextTurnPotential = 0;
    }
}
