using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// companion deals more damage for each burn on the enemy.
/// </summary>
public class OrangeCompanion : BaseCompanion
{
    int damage = 1;
    public override void PreformAction()
    {
        int dmg = 0;
        foreach(BaseStatus s in EnemyCharacter.Instance.statusEffects)
        {
            if(s is Burn)
            {
                dmg += damage;
            }
        }
        EnemyCharacter.Instance.LoseMana(dmg, companionName);
        CombatLog.Instance.AmendTextToLog($"{companionName} dealt {dmg} to {EnemyCharacter.Instance.GetCharacterName()}");
    }
    protected override string GetDescription()
    {
        return $"Deal damage for each burn on the enemy.";
    }
}
