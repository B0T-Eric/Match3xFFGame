using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[Serializable]
public abstract class BaseStatus
{
    protected BaseCharacter character;
    protected EnemyCharacter enemyCharacter;
    public Sprite Icon;

    // Notify UI or other listeners when this status changes (duration/stack/usecount etc.)
    public event Action Changed;

    protected void NotifyChanged()
    {
        Changed?.Invoke();
    }

    public virtual void PreformEffect()
    {

    }
}
public interface IPartyStatus
{
    
}
public interface IStackableStatus
{
    int StackCount { get; set; }
    void DecreaseStack();
    void IncreaseStack(int count);
    void RemoveAllStacks();
}
public interface IDurationStatus
{
    int Duration { get; set; }
    void DecreaseDuration();
    void IncreaseDuration(int duration);
}
public interface IDamageStatus
{
    int Damage { get; set; }
    void DealDamage();
}
public interface IUseStatus
{
    int UseCount { get; set; }
    void DecreaseUseCount();
    void IncreaseUseCount(int count);
}
public interface IStatItem 
{
    void DecreaseStats(int num);
    void IncreaseStats(int num);
    void OnEquip();
    void OnUnequip();
}
public interface IDebuff
{

}
public interface IBuff
{

}
public interface IOnApply
{

}
public interface IOnGetDamage
{
    public int GetDamage();
}
public interface IOnGetDamageReduction
{
    public int GetDamageReduction();
}
public interface IOnGetHealMult
{
    public float GetHealMult();
}
public interface IOnGetHealFlat
{
    public int GetHealFlat();
}
public interface IOnGetDamageMult
{
    public int GetDamageMult(int num);
}
public interface IOnGetPotentialMult
{
    public int GetPotentialMult(int num);
}
public interface IOnGetPotential
{
    public int GetPotential();
}
public static class StatusHandler
{
    //find an remove all status's with 0 or less duration/stacks since they're no longer effective
    public static List<int> CheckStatusDurations(ObservableCollection<BaseStatus> statusEffects)
    {
        List<int> toRemove = new List<int>();

        foreach (BaseStatus status in statusEffects)
        {
            if ((status is IDurationStatus d && d.Duration <= 0) ||
                (status is IStackableStatus s && s.StackCount <= 0) ||
                (status is IUseStatus u && u.UseCount <= 0))
            {
                toRemove.Add(statusEffects.IndexOf(status));
            }
        }
        return toRemove;
    }
    //remove a status effected from list on character
    public static void RemoveStatusEffect(BaseStatus status, BaseCharacter character)
    {
        if (status is IOnTurnEnd end)
        {
            character.onTurnEnd -= end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            character.onTurnStart -= start.OnTurnStart;
        }
        if (status is IOnAttackItem attack)
        {
            character.onAttack += attack.OnAttack;
        }
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            character.stats.onGetResist -= onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            character.stats.onGetDmgFlat -= getDamage.GetDamage;
        }
        if (status is IOnGetHealFlat onHeal)
        {
            character.stats.onGetHealFlat -= onHeal.GetHealFlat;
        }
        if (status is IOnGetHealMult onHealMult)
        {
            character.stats.onGetHealMult -= onHealMult.GetHealMult;
        }
        if (status is IOnGetDamageMult getDamageMult)
        {
            character.stats.onGetDmgMult -= getDamageMult.GetDamageMult;
        }
        character.statusEffects.Remove(status);
    }
    //remove a status effected from list on character
    public static void RemoveStatusEffect(BaseStatus status, EnemyCharacter character)
    {
        if (status is IOnTurnEnd end)
        {
            character.onTurnEnd -= end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            character.onTurnStart -= start.OnTurnStart;
        }
        if (status is IOnEnemyAttackItem attack)
        {
            character.onAttack -= attack.OnAttack;
        }
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            character.stats.onGetResist -= onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            character.stats.onGetDmgFlat -= getDamage.GetDamage;
        }
        if (status is IOnGetHealFlat onHeal)
        {
            character.stats.onGetHealFlat -= onHeal.GetHealFlat;
        }
        if (status is IOnGetHealMult onHealMult)
        {
            character.stats.onGetHealMult -= onHealMult.GetHealMult;
        }
        if (status is IOnGetDamageMult getDamageMult)
        {
            character.stats.onGetDmgMult -= getDamageMult.GetDamageMult;
        }
        character.statusEffects.Remove(status);
    }

    //add a parsed in status effect to the list
    //first check if it exists to then either increase stack coun (if possible) and increase duration
    public static void AddStatusEffect(BaseStatus status, BaseCharacter character)
    {
        for (int i = 0; i < character.statusEffects.Count; i++)
        {
            BaseStatus existing = character.statusEffects[i];
            if (status is Burn)
            {
                character.statusEffects.Add(status);
                return;
            }
            if (existing.GetType() == status.GetType())
            {
                if (existing is IStackableStatus existingStack && status is IStackableStatus newStack)
                {
                    existingStack.IncreaseStack(newStack.StackCount);
                }

                if (existing is IDurationStatus existingDur && status is IDurationStatus newDur)
                {
                    existingDur.IncreaseDuration(newDur.Duration);
                }

                if (existing is IUseStatus existingUse && status is IUseStatus newUse)
                {
                    existingUse.IncreaseUseCount(newUse.UseCount);
                }
                return;
            }
        }
        character.statusEffects.Add(status);
        if (status is IOnTurnEnd end)
        {
            character.onTurnEnd += end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            character.onTurnStart += start.OnTurnStart;
        }
        if (status is IOnDamageTakenItem dmg)
        {
            character.onDamageTaken += dmg.OnDamageTaken;
        }
        if (status is IOnAttackItem attack)
        {
            character.onAttack += attack.OnAttack;
        }
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            character.stats.onGetResist += onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            character.stats.onGetDmgFlat += getDamage.GetDamage;
        }
        if (status is IOnGetHealFlat onHeal)
        {
            character.stats.onGetHealFlat += onHeal.GetHealFlat;
        }
        if (status is IOnGetHealMult onHealMult)
        {
            character.stats.onGetHealMult += onHealMult.GetHealMult;
        }
        if (status is IOnApply)
        {
            status.PreformEffect();
        }
        if (status is IOnGetDamageMult getDamageMult)
        {
            character.stats.onGetDmgMult += getDamageMult.GetDamageMult;
        }
    }
    //add a parsed in status effect to the list
    //first check if it exists to then either increase stack coun (if possible) and increase duration
    public static void AddStatusEffect(BaseStatus status, EnemyCharacter character)
    {
        for (int i = 0; i < character.statusEffects.Count; i++)
        {
            BaseStatus existing = character.statusEffects[i];
            if (status is Burn)
            {
                character.statusEffects.Add(status);
                return;
            }
            if (existing.GetType() == status.GetType())
            {
                if (existing is IStackableStatus existingStack && status is IStackableStatus newStack)
                {
                    existingStack.IncreaseStack(newStack.StackCount);
                }

                if (existing is IDurationStatus existingDur && status is IDurationStatus newDur)
                {
                    existingDur.IncreaseDuration(newDur.Duration);
                }

                if (existing is IUseStatus existingUse && status is IUseStatus newUse)
                {
                    existingUse.IncreaseUseCount(newUse.UseCount);
                }
                return;
            }
        }
        character.statusEffects.Add(status);
        if (status is IOnTurnEnd end)
        {
            character.onTurnEnd += end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            character.onTurnStart += start.OnTurnStart;
        }
        if (status is IOnEnemyDamageTakenItem dmg)
        {
            character.onDamageTaken += dmg.OnDamageTaken;
        }
        if (status is IOnEnemyAttackItem attack)
        {
            character.onAttack += attack.OnAttack;
        }
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            character.stats.onGetResist += onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            character.stats.onGetDmgFlat += getDamage.GetDamage;
        }
        if (status is IOnGetHealFlat onHeal)
        {
            character.stats.onGetHealFlat += onHeal.GetHealFlat;
        }
        if (status is IOnGetHealMult onHealMult)
        {
            character.stats.onGetHealMult += onHealMult.GetHealMult;
        }
        if (status is IOnApply)
        {
            status.PreformEffect();
        }
        if (status is IOnGetDamageMult getDamageMult)
        {
            character.stats.onGetDmgMult += getDamageMult.GetDamageMult;
        }
    }
}