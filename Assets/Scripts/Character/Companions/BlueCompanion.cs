using UnityEngine;
/// <summary>
/// this companion increases stacks on enemy statuses.
/// </summary>
public class BlueCompanion : BaseCompanion
{
    private int StackIncrease = 1;
    public override void PreformAction()
    {
        foreach(BaseStatus s in EnemyCharacter.Instance.statusEffects)
        {
            if(s is IStackableStatus stackable)
            {
                if(s is not Hunger)
                stackable.IncreaseStack(StackIncrease);
                CombatLog.Instance.AmendTextToLog($"{companionName} increased {stackable.GetType().Name} stacks by {StackIncrease}");
            }
        }
    }
    protected override string GetDescription()
    {
        return $"Increases stacks by {StackIncrease} on each negative status affecting the enemy.";
    }
}
