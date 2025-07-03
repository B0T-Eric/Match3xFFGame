using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Blue character class facilitates the unique passive effects and features of the blue character.
/// The unique attributes of the Blue Character are the ability to take advantage of enemies who are debuffed or buffed by status effects.
/// Passive Level 1: Gain 5 more damage when an enemy has a status effect.
/// Passive Level 2: Gain 10 more damage when an enemy has a status effect.
/// Passive Level 3: Tbd.
/// </summary>
[Serializable]
public class BlueCharacter : BaseCharacter
{
    public override void Init()
    {
        manaType = ManaType.Blue;
        hexColour = "000CFF";
        base.Init();    
    }

    protected override int GetDamage()
    {
        int damage = base.GetDamage();
        if (EnemyCharacter.Instance.statusEffects.Count > 0) 
        {
            if(currentMana > secPassiveThreshold)
            {
                damage += 10;
            }
            else
            {
                damage += 5;
            }
        }
        return damage; 
    }

}
