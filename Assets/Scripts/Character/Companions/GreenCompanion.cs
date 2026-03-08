using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCompanion : BaseCompanion
{
    private int HealAmount = 1;
    public override void PreformAction()
    {
        pairedCharacter.HealMana(HealAmount);
        CombatLog.Instance.AmendTextToLog($"{companionName} heals {pairedCharacter.GetCharacterName()} for {HealAmount}");
    }
    protected override string GetDescription()
    {
        return $"Restores {1} spirit mana. (is affected by healing modifiers)";
    }
}
