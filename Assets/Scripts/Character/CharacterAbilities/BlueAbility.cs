using System.Collections;
using UnityEngine;
/// <summary>
/// Blue Ability: Trap enemy in a bubble, which increases damage dealt by the next attack, or detonates for damage at the start of the next turn.
/// Cost: 20 mana.
/// Cooldown: 1 turns;
/// </summary>
public class BlueAbility : BaseAbility
{
    [SerializeField]
    Sprite bubbleSprite;
    public override void ActivateAbility(BaseCharacter target)
    {
        base.ActivateAbility();
        Bubble b = new(bubbleSprite, target);
        target.AddStatusEffect(b);
    }
}