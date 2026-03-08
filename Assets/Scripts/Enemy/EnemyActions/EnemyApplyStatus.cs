using System.Collections.Generic;
using UnityEngine;

public class EnemyApplyStatus : EnemyAction
{
    BaseStatus status;
    /// <summary>
    /// apply status to single target.
    /// </summary>
    /// <param name="target"> the chracter having the status applied too it.</param>
    public override void PreformAction(BaseCharacter target)
    {
        target.AddStatusEffect(status);
    }
    public override void PreformAction(List<BaseCharacter> targets)
    {
        foreach (BaseCharacter target in targets)
        {
            target.AddStatusEffect(status);
        }
    }
    /// <summary>
    /// Define status being applied.
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(BaseStatus status)
    {
        this.status = status;
    }
}