using System.Collections;
using UnityEngine;

/// <summary>
/// High damage meteor that deals 100 total damage across 2 instances, 1 on cast and the other one burn trigger.
/// cooldown: 2 turns.
/// cost: 20 mana.
/// </summary>
public class OrangeAbility : BaseAbility
{
    [SerializeField]
    int duration;
    [SerializeField]
    int damage;
    [SerializeField]
    Sprite icon;
    public override void ActivateAbility(BaseCharacter target)
    {
        base.ActivateAbility();
        Burn B = new(duration, damage, target, icon);
        target.LoseMana(damage,this.GetType().Name);
        target.AddStatusEffect(B);
    }
}
