using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public abstract class BaseStatus
{
    protected BaseCharacter character;
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
public interface IOnGetHealing
{
    public int GetHealing();
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