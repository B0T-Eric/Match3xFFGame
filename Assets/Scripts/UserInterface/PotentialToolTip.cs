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
            $"\nPotential retained this turn = {Mathf.RoundToInt(character.potential * character.GetRetainMult_Public())}" +
            $"\nAmount of potential retained whenever potential is retained in combat." +
            $"\nFormula = current potential * potential retain multiplier \n" +
            $"\nCurrent potential = {character.potential}." +
            $"\nAmount of potential gained from matches during turns." +
            $"\nFormula = current potential + flat potential increase * potential multiplier.";
    }
}
