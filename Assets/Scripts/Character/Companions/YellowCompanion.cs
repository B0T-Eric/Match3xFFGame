using UnityEngine;

/// <summary>
/// For each 5 potential deal 1 damage each turn.
/// </summary>
public class YellowCompanion : BaseCompanion
{
    int threshold = 4;
    public override void PreformAction()
    {
        int damage = pairedCharacter.potential % threshold;
        EnemyCharacter.Instance.LoseMana(damage, companionName);
        CombatLog.Instance.AmendTextToLog($"{companionName} dealt {damage} damage to {EnemyCharacter.Instance.GetCharacterName()}");
    }
    protected override string GetDescription()
    {
        return $"For every {threshold} potential {pairedCharacter.GetCharacterName()} has, deal 1 damage. (as a single instance)";
    }
}
