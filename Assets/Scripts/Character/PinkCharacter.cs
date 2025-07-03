using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private BaseStatus[] statuses;
    private float[] modifiers = { 0.5f, 1.5f, 2f, 2.5f };
    public override void Init()
    {
        statuses = new BaseStatus[] { new Rage(rageSprite), new Passion(passionSprite) };
        manaType = ManaType.Pink;
        hexColour = "FF00BE";
        base.Init();
    }
    public override void Attack(BaseCharacter c)
    {
        if (currentMana < secPassiveThreshold)
        {
            base.Attack(c);
        }
        //level 2 passive attacks 2 times with variably amped damage.
        else
        {
            if (state == CharacterState.Alive)
            {
                int damage = GetDamage();
                damage /= 2;
                int firstHit = GetDamageSpecial(damage);
                int secondHit = GetDamageSpecial(damage);

                c.LoseMana(firstHit);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c);
                }
                c.LoseMana(secondHit);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c);
                }

            }
            nextTurnPotential = 0;
        }
    }
    private int GetDamageSpecial(int damage)
    {
        int rng = Random.Range(0, 101);
        if(rng >= 0 && rng < 30)
        {
            return Mathf.RoundToInt(damage * modifiers[0]);
        }
        else if(rng >= 30 && rng < 55)
        {
            return Mathf.RoundToInt(damage * modifiers[1]);
        }
        else if(rng >= 55 && rng < 75)
        {
            return Mathf.RoundToInt(damage * modifiers[2]);
        }
        else if(rng >= 75 && rng < 90)
        {
            return Mathf.RoundToInt(damage * modifiers[3]);
        }
        else
        {
            return damage;
        } 
    }
    protected override int GetDamage()
    {
        int damage = potential;
        //status based damage amp / conditional damage from abilities or other party members
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.flatDamageIncrease > 0)
            {
                damage += item.itemSO.flatDamageIncrease;
            }
            if (item.itemSO.damageMult > 0)
            {
                damage = (int)(damage * item.itemSO.damageMult);
            }
        }
        if (statusEffects.Contains(statuses[0]))
        {
            Rage r = (Rage)statuses[0];
            damage = Mathf.RoundToInt(damage * r.OutgoingDamageMultiplier);
        }
        if(damage > 10 && currentMana > thirdPassiveThreshold)
        {
            base.HealMana(Mathf.RoundToInt(damage * 0.25f));
        }
        return damage;
    }
    public override void GainPotential(int amount)
    {
        if (statusEffects.Contains(statuses[1]))
        {
            Passion passion = (Passion)statuses[1];
            amount = Mathf.RoundToInt(amount * passion.PotentialGainMult);
        }
        base.GainPotential(amount);
    }
    public override void TurnStart()
    {
        int rng = Random.Range(0,statuses.Length);
        AddStatusEffect(statuses[rng]);
        base.TurnStart();
    }
}
