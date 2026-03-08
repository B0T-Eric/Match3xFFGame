using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// paired character preforms an additional attack if potential is higher than 10
/// </summary>
public class PinkCompanion : BaseCompanion
{
    int threshold = 10;
    public override void PreformAction()
    {
        if(pairedCharacter.potential > threshold)
        {
            pairedCharacter.Attack(EnemyCharacter.Instance);
            CombatLog.Instance.AmendTextToLog($"{companionName} gave {pairedCharacter.GetCharacterName()} an extra attack.");
        }
    }
}
