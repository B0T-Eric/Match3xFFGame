using System.Collections;
using UnityEngine;
/// <summary>
/// Stun the enemy for 1 turn.
/// cooldown: 1.
/// Cost: 15.
/// </summary>
public class YellowAbility : BaseAbility
{
    public override void ActivateAbility(BaseCharacter target)
    {
        base.ActivateAbility();
        target.isStunned = true;
    }
}