using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/// <summary>
/// Blue character class facilitates the unique passive effects and features of the blue character.
/// The unique attributes of the Blue Character are the ability to take advantage of enemies who are debuffed or buffed by status effects.
/// Passive Level 1: Gain 5 more damage when an enemy has a status effect.
/// Passive Level 2: Gain 10 more damage when an enemy has a status effect.
/// Passive Level 3: foreach unique status on the enemy character deal another +3 damage on attack.
/// </summary>
[Serializable]
public class BlueCharacter : BaseCharacter
{
    public override void Init()
    {
        manaType = ManaType.Blue;
        hexColour = "000CFF";
        base.Init();
        description = $"<b><color=#{hexColour}> {manaType} Character </b></color>\n Gain potential via matching the {manaType} spirit tokens. " +
            $"\n <b>Passives: \n</b> Passive 1 (0 mana): Deal +5 bonus damage on attack to enemies who have a status effect applied to them." +
            $"\n Passive 2 ({passiveValues.secPassiveThreshold} mana): Deal an additional +5 bonus damage ontop of the first pasives bonus damage." +
            $"\n Passive 3 ({passiveValues.thirdPassiveThreshold} mana): For each status affecting the enemy deal +3 bonus damage on attack.";
       
    }

    protected override int GetDamage()
    {
        int damage = base.GetDamage();
        if (EnemyCharacter.Instance.statusEffects.Count > 0) 
        {
            if(currentMana > passiveValues.secPassiveThreshold)
            {
                damage += 10;
            }
            else
            {
                damage += 5;
            }
        }
        if (currentMana > passiveValues.thirdPassiveThreshold)
        {
            foreach(BaseStatus status in EnemyCharacter.Instance.statusEffects)
            {
                damage += 3;
            }
        }
        return damage; 
    }

}
