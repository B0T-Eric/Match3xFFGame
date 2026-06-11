using System.Collections;
using UnityEngine;

/// <summary>
/// *logic handled in character class*
/// Pink character meditates and enters a peaceful stance, if attacked while in the stance, they become enraged, if not attack while in the stance become passionate.
/// passion = 2x potentail gain.
/// enrage = 1.5x damage;
/// cooldown: 1.
/// cost: 0;
/// </summary>
public class PinkAbility : BaseAbility
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        CombatLog.Instance.AmendTextToLog($"{character.GetCharacterName()} has begun meditating.");
    }
    public override string GetDescription()
    {
        return $"Meditate:" +
            $"\nEnter a peaceful trance, if attacked gain rage, if not gain passion." +
            $"\nAffects apply next turn." +
            $"\nCost: {cost}    Cooldown: {abilityCooldown}";
    }
}
