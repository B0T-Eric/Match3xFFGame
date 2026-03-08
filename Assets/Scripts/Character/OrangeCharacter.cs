using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Orange character class facilitates the unique ablities and evolutions of the orange character type.
/// The primary unique effect of Orange Character is to apply burn status effects to amplify their damage output.
/// Burn is not unique to Orange character - can be applied by Items or Enemies.
/// Passive level 1: 
/// Passive level 2: 
/// Passive level 3: 
/// </summary>
public class OrangeCharacter : BaseCharacter
{
    public Sprite Icon,SuperFireIcon;
    private readonly float burnDamagePercent = 1f;
    private readonly int burnDuration = 2;
    private readonly int burnBonusDamage = 5;
    [SerializeField]
    private FireStormAttack fsa;
    public override void Init()
    {
        manaType = ManaType.Orange;
        hexColour = "FF8000";
        onAttack += OnAttack;
        base.Init();

    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Attacks set enemies on fire, applying the burn status effect for {burnDamagePercent * 100}% damage.\n " +
            $"Every 5th multiple of damage from the original attack adds {burnBonusDamage} to the applied burn.";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Basic attack upgraded to deal more damage based on burns affecting the enemy.\n" +
            $"Additionally, gain a new attack that deals no direct damage instead applies 1 burn per 7 damage that would be done.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"Upgrades burns to Super burns which increases damage by 25% once every turn (until it expires).";
    }
    #endregion
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility(EnemyCharacter.Instance);
    }
    //on attack from orange character, take half the damage delt, and the enemy and apply a burn effect.
    //each burn is independant
    public void OnAttack(BaseCharacter character, int damage)
    {
        int bonusDamage = Mathf.RoundToInt(damage/burnBonusDamage);
        int burnDamage = Mathf.RoundToInt(damage * burnDamagePercent + bonusDamage);
        if (burnDamage > 0)
        {
            if (IsPassiveThreeActive)
            {
                SuperBurn b = new SuperBurn(burnDuration, burnDamage, character, SuperFireIcon);
                character.AddStatusEffect(b);
            }
            else
            {
                Burn b = new Burn(burnDuration, burnDamage, character, Icon);
                character.AddStatusEffect(b);
            }
        }
    }
    protected override void UpdatePassives()
    {
        if (IsPassiveTwoActive)
        {
            if (!attacks.Contains(fsa))
            {
                attacks.Add(fsa);
            }
        }
        else
        {
            if (attacks.Contains(fsa))
            {
                attacks.Remove(fsa);
                selectedAttack = 0;
            }
        }
    }
    public override void Attack(BaseCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            int damage = GetDamage();
            if (damage > 0)
            {
                if (selectedAttack == 0)
                {
                    if (IsPassiveTwoActive)
                    {
                        damage *= EnemyCharacter.Instance.statusEffects.Where(status => status is Burn).Count();
                    }
                    attacks[selectedAttack].Attack(c, damage, this, GetCharacterName(), onAttack);
                }
                else if (selectedAttack == 1)
                {
                    BaseStatus status;
                    if (IsPassiveThreeActive)
                    {
                        status = new SuperBurn(burnDuration, 0, c, Icon);
                    }
                    else
                    {
                        status = new Burn(burnDuration,0,c, Icon);
                    }
                    attacks[selectedAttack].Attack(c, damage, this, GetCharacterName(), onAttack, status);
                }
            }
        }
        nextTurnPotential = 0;
    }
}
