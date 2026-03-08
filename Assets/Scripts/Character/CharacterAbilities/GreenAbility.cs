using System.Collections;
using UnityEngine;


/// <summary>
/// GreenAbility: Cleanse (remove) debuffs from all characters in party.
/// Cost: 12 mana
/// Cooldown: 3 turns
/// </summary>
public class GreenAbility : BaseAbility
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        foreach(BaseCharacter character in Party.Instance.members)
        {
            for(int i = 0; i < character.statusEffects.Count; i++)
            {
                if(character.statusEffects[i] is IDebuff)
                {
                    character.statusEffects.RemoveAt(i);
                }
            }
        }
    }
}
