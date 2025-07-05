using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCharacter : BaseCharacter
{
    public int ReturnDamage;
    public int RegenHealth;
    public override void Init()
    {
        manaType = ManaType.Purple;
        hexColour = "AD00FF";
        base.Init();
    }
    public override void LoseMana(int amount)
    {
        base.LoseMana(amount);
        if(currentMana > secPassiveThreshold)
        {
            HealMana(RegenHealth);
        }
        if(currentMana > thirdPassiveThreshold)
        {
            EnemyCharacter.Instance.LoseMana(ReturnDamage);
        }
    }
}
