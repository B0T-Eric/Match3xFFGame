using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReductionTooltip : ToolTip
{
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
    }
    private void Update()
    {
        Tooltip = $"<color=grey>Defence Stats</color>\n" +
            $"Damage Reduction = {character.GetDamageReduction_Public()}." +
            $"\nDamage reduction lowers damage taken by the specified amount." +
            $"\nFormula = Damage Incoming ({IncomingDamage()}) - Damage Reduction ({character.GetDamageReduction_Public()}).";
    }
    private string IncomingDamage()
    {
        if (EnemyCharacter.Instance != null)
        {
            if (Party.Instance.HasKnowledgeOfEnemy)
            {
                return EnemyCharacter.Instance.GetDamage_Public().ToString();
            }
            else
            {
                return "?";
            }
        }
        else
        {
            return "?";
        }
    }
}