using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
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
    [SerializeField]
    private Sprite freezeSprite;
    [SerializeField]
    private int freezeDamageReduction;
    [SerializeField]
    private int freezeArmourReduction;
    private float percentAmp = 1.15f;
    private int flatAmp = 3;
    private float p3Amp = 1.25f;
    private int FreezeDuration = 1;
    public override void Init()
    {
        manaType = ManaType.Blue;
        hexColour = "000CFF";
        stats = new BlueStats(statsSO.maxMana, statsSO.initialMana, statsSO.passiveTwo, statsSO.passiveThree, 0.75f, flatAmp, percentAmp, p3Amp);
        base.Init();
       
    }
    public void OnHit(BaseCharacter character)
    {
        if (IsPassiveTwoActive)
        {
            int debuffCount = EnemyCharacter.Instance.statusEffects.Where(status => status is IDebuff).Count();
            if (debuffCount > 3)
            {
                if(IsPassiveThreeActive)
                {
                    Freeze f = new Freeze(freezeSprite ,freezeDamageReduction, freezeArmourReduction,FreezeDuration);
                    StatusHandler.AddStatusEffect(f, EnemyCharacter.Instance);
                }
                else
                {
                    Freeze f = new Freeze(freezeSprite, freezeDamageReduction,FreezeDuration);
                    StatusHandler.AddStatusEffect(f, EnemyCharacter.Instance);
                }
            }
        }
        
    }
    #region descriptions
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility(EnemyCharacter.Instance);
    }
    public override string GetPassiveOneDesc()
    {
        return $"Increased damage delt by {flatAmp} or {(percentAmp -1) * 100}% (takes the highest increase) per debuff affecting the enemy.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Apply <color=#000CFF>Freeze</color> on hit, which lowers damage by {freezeDamageReduction} for {FreezeDuration} turns.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Freezes now lower damage and reduce enemy damage reduction by {freezeDamageReduction}. \nDamage is increased by an additional {(p3Amp -1 )*100}% when the enemy is affected by a debuff";
    }
    #endregion

}
