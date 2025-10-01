using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public abstract class BaseStatus
{
    protected BaseCharacter character;
    public Sprite Icon;
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
