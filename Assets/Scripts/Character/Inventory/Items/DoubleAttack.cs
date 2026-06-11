using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This item will tell an ally character in a specific position in the party 
/// to attack (for free (no potential cost))
/// if the party does not have a character at specified location, do another attack from this character.
/// </summary>
public class DoubleAttack : BaseItem, IOnAttackItem, IOnTurnStart
{
    [SerializeField]
    int characterIndex;
    [SerializeField]
    int useCount;
    public void OnAttack(EnemyCharacter character, int damage)
    {
        BaseCharacter targetCharacter = Party.Instance.members[characterIndex];
        BaseCharacter originCharacter = itemHolder;
        if (targetCharacter != null && useCount >= 1)
        {
            targetCharacter.Attack(character);
            useCount = 0;
        }

    }
    public override string GetEffectDesc()
    {
        return $"Once a turn, when the character with this item equiped attacks, the character at position {characterIndex +1} attacks immediately after for free.";
    }

    public void OnTurnStart()
    {
        useCount = 1;
    }
}
