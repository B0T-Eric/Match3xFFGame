using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Burn : BaseStatus, IDurationStatus, IDamageStatus, IOnTurnEnd, IDebuff
{
    public Burn(int duration, int damage, BaseCharacter target, Sprite icon)
    {
        Duration = duration;
        Damage = damage;
        base.Icon = icon;
        base.character = target;
    }
    public override string ToString()
    {
        return $"Burning for {Damage} damage every turn for {Duration} turns.";
    }
    public int Damage { get; set; }
    public int Duration { get; set; }

    public virtual void DealDamage()
    {
        if (Duration > 0)
        {
            character.LoseMana(Damage, this.GetType().Name);
        }
    }
    public override void PreformEffect()
    {
        DealDamage();
    }
    public void DecreaseDuration()
    {
        Duration--;
        NotifyChanged();
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
        NotifyChanged();
    }

    public void OnTurnEnd()
    {
        PreformEffect();
    }
}
public class SuperBurn : Burn
{
    private readonly float DamageGain = 0.25f;
    private int durationDecreased = 0;
    private int startDuration;
    public SuperBurn(int duration, int damage, BaseCharacter target, Sprite icon) : base(duration, damage, target, icon)
    {
        startDuration = duration;
    }
    public override void DealDamage()
    {
        if (Duration > 0)
        {
            int increasedDamage;
            if (durationDecreased > 0)
            {
                increasedDamage = Mathf.RoundToInt(Damage * (durationDecreased * DamageGain));
            }
            else
            {
                increasedDamage = Damage;
            }
            character.LoseMana(increasedDamage, this.GetType().Name);
        }
    }
    public new void DecreaseDuration()
    {
        Duration--;
        durationDecreased++;
        NotifyChanged();
    }
    public override string ToString()
    {
        return $"Burning for {Damage} damage every turn for {Duration} ({startDuration} start) turns,\n each turn passed increases damage by {DamageGain*100}%.";
    }
}
public class Weakness : BaseStatus, IDurationStatus, IDebuff
{
    public int Duration { get; set;}

    public void DecreaseDuration()
    {
        Duration--;
        NotifyChanged();
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
        NotifyChanged();
    }
    public float OutgoingDamageModifier;
    public Weakness(int duration, float modifier, Sprite sprite)
    {
        Icon = sprite;
        Duration = duration;
        OutgoingDamageModifier = modifier;
    }
    public float ApplyWeakness()
    {
        return OutgoingDamageModifier;
    }
    public override string ToString()
    {
        return $"Lowers outgoing damage by {OutgoingDamageModifier}x for {Duration} turns.";
    }
}
public class Brittle : BaseStatus, IUseStatus, IDebuff
{
    public int UseCount { get; set; }

    public void DecreaseUseCount()
    {
        UseCount--;
        NotifyChanged();
    }

    public void IncreaseUseCount(int count)
    {
        UseCount += count;
        NotifyChanged();
    }
    public float IncomingDamageMultiplier;
    public Brittle(int useCount, float mult, Sprite sprite)
    {
        Icon = sprite;
        UseCount = useCount;
        IncomingDamageMultiplier = mult;
    }
    public float ApplyBrittle()
    {
        DecreaseUseCount();
        return IncomingDamageMultiplier;
    }
    public override string ToString()
    {
        return $"Increases incoming damage by {IncomingDamageMultiplier}x for {UseCount} attacks.";
    }
}
public class Rage : BaseStatus, IDurationStatus, IBuff, IOnGetDamageMult
{
    public int Duration { get; set; }

    public void DecreaseDuration()
    {
        Duration--;
        NotifyChanged();
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
        NotifyChanged();
    }
    public float OutgoingDamageMultiplier;
    public Rage(Sprite sprite)
    {
        Icon=sprite;
        Duration = 1;
        OutgoingDamageMultiplier = .5f;
    }   
    public override string ToString()
    {
        return $"Increases outgoing damage by {OutgoingDamageMultiplier*100}% for {Duration} turns.";
    }

    public int GetDamageMult(int num)
    {
        return Mathf.RoundToInt(num * OutgoingDamageMultiplier);
    }
}
public class Passion : BaseStatus, IDurationStatus, IBuff, IOnGetPotentialMult
{
    public int Duration { get; set; }

    public void DecreaseDuration()
    {
        Duration--;
        NotifyChanged();
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
        NotifyChanged();
    }
    public float PotentialGainMult;
    public Passion(Sprite sprite)
    {
        Icon = sprite;
        Duration = 1;
        PotentialGainMult = 2f;
    }
    public override string ToString()
    {
        return $"Increases potential by {PotentialGainMult}x for {Duration} turns.";
    }

    public int GetPotentialMult(int num)
    {
        return Mathf.RoundToInt(num * PotentialGainMult);
    }
}
public class Poison : BaseStatus, IStackableStatus, IDebuff
{
    public int StackCount { get; set; }

    public void DecreaseStack()
    {
        StackCount--;
        NotifyChanged();
    }

    public void IncreaseStack(int count)
    {
        StackCount += count;    
        NotifyChanged();
    }
    public void RemoveAllStacks()
    {
        StackCount = 0;
        NotifyChanged();
    }
    public Poison(Sprite sprite)
    {
        Icon = sprite;
        StackCount = 1;
    }
    public override string ToString()
    {
        return $"A stacking debuff which can have varied affects based on items." +
            $"\nCurrent Stacks: {StackCount}.";
    }
}
public class Hunger : BaseStatus, IStackableStatus, IBuff
{
    public int StackCount { get; set; }
    public void DecreaseStack()
    {
        StackCount--;
        NotifyChanged();
    }

    public void IncreaseStack(int count)
    {
        StackCount += count;
        NotifyChanged();
    }

    public void RemoveAllStacks()
    {
        StackCount = 0;
        NotifyChanged();
    }
    public Hunger(Sprite sprite, int stacks)
    {
        StackCount = stacks;
        Icon = sprite;
        NotifyChanged();
    }
    public override string ToString()
    {
        return $"Each stack of Hunger provides 10% increased damage.\nCurrent damage boost:{StackCount /10}%";
    }
}
public class Wound : BaseStatus, IStackableStatus, IDurationStatus,  IOnHitItem, IDebuff
{
    public int StackCount { get; set; }
    public int Duration { get; set; }
    public Wound(Sprite sprite, int duration)
    {
        Icon = sprite;
        StackCount = 1;
        Duration = duration;
    }
    public void DecreaseDuration()
    {
        Duration--;
        NotifyChanged();
    }

    public void DecreaseStack()
    {
        StackCount--;
        NotifyChanged();
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
        NotifyChanged();
    }

    public void IncreaseStack(int count)
    {
        StackCount += count;
        NotifyChanged();
    }

    public void RemoveAllStacks()
    {
        StackCount = 0;
        NotifyChanged();
    }

    public void OnHit(BaseCharacter character)
    {
        character.LoseMana(1 * StackCount, this.GetType().Name);
    }
    public override string ToString()
    {
        return $"On hit, take additional damage based on the amount of stacks.\nCurrent damage boost {1 * StackCount}.";
    }
}

public class ExchangeEffect : BaseStatus, IPartyStatus, IBuff
{
    ManaType Type;

    public ExchangeEffect(Sprite sprite, ManaType type)
    {
        Icon = sprite;
        Type = type;
    }

    public override void PreformEffect()
    {
        foreach (BaseCharacter c in Party.Instance.members)
        {
            c.maxMana -= 10;
            if (c.currentMana > c.maxMana)
            {
                c.currentMana = c.maxMana;
            }
        }
        RemoveMana();
    }

    private void RemoveMana()
    {
        TileState state = TileState.A;
        switch (Type)
        {
            case ManaType.Yellow:
                state = TileState.A;
                break;
            case ManaType.Green:
                state = TileState.B;
                break;
            case ManaType.Orange:
                state = TileState.C;
                break;
            case ManaType.White:
                state = TileState.D;
                break;
            case ManaType.Blue:
                state = TileState.E;
                break;
            case ManaType.Purple:
                state = TileState.F;
                break;
            case ManaType.Pink:
                state = TileState.G;
                break;
            default:
                Debug.LogError("Invalid ManaType provided.");
                break;
        }
        Party.Instance.RemoveTileType(state);
    }
    public override string ToString()
    {
        return $"Lowered party health by 10, Removed {Type} spirit tokens from pool.";
    }
}
public class Bubble : BaseStatus, IDebuff, IOnDamageTakenItem, IUseStatus, IOnTurnStart, IOnApply
{
    public int baseDamage = 20;
    public float damageMultiplier = 2.5f;

    public int UseCount { get; set; }

    public override void PreformEffect()
    {
        character.isStunned = true;
    }

    public void OnDamageTaken(BaseCharacter baseCharacter, int damage)
    {
        baseCharacter.LoseMana(Mathf.RoundToInt(damage * 2.5f), this.GetType().Name);
        character.isStunned = false;
        DecreaseUseCount();
    }

    public void OnTurnStart()
    {
        character.LoseMana(baseDamage, this.GetType().Name);
        character.isStunned = false;
        DecreaseUseCount();
    }

    public void DecreaseUseCount()
    {
        UseCount--;
    }

    public void IncreaseUseCount(int count)
    {
        UseCount += count;
    }

    public Bubble(Sprite sprite, BaseCharacter target)
    {
        Icon = sprite;
        UseCount = 1;
        character = target;
    }
}
//reduce damage and reduce damage resistance
public class Freeze : BaseStatus, IDebuff, IOnGetDamage, IOnGetDamageReduction, IDurationStatus
{
    int damage;
    int damageReduction;

    public int Duration { get; set; }

    public Freeze(Sprite sprite, BaseCharacter target, int damage, int damageReduction, int duration)
    {
        Icon = sprite;
        this.damage = damage;
        this.damageReduction = damageReduction;
        Duration = duration;
    }
    public Freeze(Sprite sprite, BaseCharacter target, int damage, int duration)
    {
        Icon = sprite;
        this.damage = damage;
        damageReduction = 0;
        Duration = duration;
    }
    public int GetDamage()
    {
        return damage;
    }
    public int GetDamageReduction()
    {
        return damageReduction;
    }

    public void DecreaseDuration()
    {
        Duration--;
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
    }
}