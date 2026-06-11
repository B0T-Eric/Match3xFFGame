using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.UI.Image;
/// <summary>
/// Pink character classes purpose is to facilitate the unique buffs and attacks of the pink character.
/// The unique effects of pink character are their unique statuses which change mana generation and damage delt.
/// Passive level 1: On turn start, apply either Rage (50% outgoing damage) or Passion (2x mana gained).
/// Passive level 2: Attacks damage is now randomised (Variable damage - non-zero), and can be triggered 2 times.
/// Passive level 3: Heals 25% of damage dealt if damage is above 10.
/// </summary>
public class PinkCharacter : BaseCharacter
{
    public Sprite rageSprite, passionSprite;
    [SerializeField]
    PinkSpecialAttack spec;
    private bool stanceActive = false;
    public override void Init()
    {
        manaType = ManaType.Pink;
        hexColour = "FF00BE";
        onAttack += OnAttack;
        attacks.Clear();
        attacks.Add(spec);
        stats = new PinkStats(statsSO.maxMana, statsSO.initialMana, statsSO.passiveTwo, statsSO.passiveThree, 0.75f);
        base.Init();
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"Under specific conditions call upon the aid of their bodies occupants to empower their attack." +
            $"\n<b>Empowered Conditions</b> - When initial damage is a multiple of:\n" +
            $"3 (6,9,12,15 etc...), 4 (8,12,16,20 etc...) and 6 (12,18,24,30 etc...).\n" +
            $"If second passive is active an additional condition is added: 5 (10,15,20,25 etc...)." +
            $"\n<b>Increases to damage:</b>\n" +
            $"When initial damage is a multiple of 3 or more conditions; damage is increased by 200%\n" +
            $"When initial damage is a multiple of 2 of 3 conditions; damage is increased by 100%\n" +
            $"When initial damage is a multiple of 1 or none of the conditions; damage is not increased";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"Empowered attacks are no longer increased in damage, instead the damage is split across multiple hits\nA new condition is added to condition pool`.";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"If damage is less than 10 damage becomes 10.\nDamage greater than 20 heals for 25% of the damage dealt.";
    }
    #endregion
    public override void Attack(EnemyCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            int damage = stats.GetDamage(potential);
            if (damage > 0)
            {
                if (selectedAttack == 0)
                {
                    attacks[selectedAttack].Attack(c, damage, this, GetCharacterName(), onAttack);
                }
   
            }
        }
        nextTurnPotential = 0;
    }
    private void OnAttack(EnemyCharacter c, int damage)
    {
        if (damage > 20 && IsPassiveThreeActive)
        {
            base.HealMana(Mathf.RoundToInt(damage * 0.25f));
        }
    }
    protected override void AbilityButtonClick()
    {
        Ability.ActivateAbility();
        stanceActive = true;
        onHit += OnHit;
    }
    public void OnHit(EnemyCharacter character)
    {
        if (stanceActive)
        {
            GetBuff(1);
            onHit -= OnHit;
            stanceActive = false;
        }
    }
    private void GetBuff(int num)
    {
        switch (num)
        {
            case 0: StatusHandler.AddStatusEffect(new Rage(rageSprite), this); break;
            case 1: StatusHandler.AddStatusEffect(new Passion(passionSprite), this); break;
        }
    }

    public override void TurnStart()
    {
        if(stanceActive)
        {
            GetBuff(1);
            stanceActive = false;
        }
        base.TurnStart();
    }
}
