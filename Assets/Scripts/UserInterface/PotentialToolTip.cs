using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialToolTip : ToolTip
{
    BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
    }
    private void Update()
    {
        Tooltip = $"<color=#00FFFF>Potential Stats</color>\n" +
            $"\nPotential retained this turn = {character.stats.GetNextTurnPotential(character.potential)}" +
            $"\nAmount of potential retained whenever potential is retained in combat." +
            $"\nFormula = number of tiles matched + flat potential increase * potential multiplier ." +
            $"\nCurrent potential = {character.potential}." +
            $"\nAmount of potential gained from matches during turns." +
            $"\nFormula = number of tiles matched + flat potential increase * potential multiplier .";
    }
}
