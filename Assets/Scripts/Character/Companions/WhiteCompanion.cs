using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a weakness to the  enemy every second turn.
/// </summary>
public class WhiteCompanion : BaseCompanion
{
    [SerializeField]
    Sprite weaknessSprite;
    Weakness weakness;
    int duration = 1;
    float modifier = 0.75f;
    bool wSwitch = false;
    public override void PreformAction()
    {
        if (wSwitch)
        {
            StatusHandler.AddStatusEffect(weakness,EnemyCharacter.Instance);
            CombatLog.Instance.AmendTextToLog($"{companionName} applied {modifier * 100}% Weakness for {weakness.Duration}");
        }
        wSwitch = !wSwitch;
    }
    public override void Init(BaseCharacter c)
    {
        base.Init(c);
        weakness = new(duration, modifier, weaknessSprite);
    }
    protected override string GetDescription()
    {
        return $"Applies {duration} turn of weakness every second turn.\nWeakness lowers damage by {(modifier) * 100}%";
    }
}
