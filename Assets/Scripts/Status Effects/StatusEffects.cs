using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : BaseStatus, IDurationStatus, IDamageStatus, IOnTurnEnd
{
    public Burn(int duration, int damage, BaseCharacter character, Sprite icon)
    {
        Duration = duration;
        Damage = damage;
        base.Icon = icon;
        base.character = character;
    }
    public override string ToString()
    {
        return $"Burning for {Damage} damage every turn for {Duration} turns.";
    }
    public int Damage { get; set; }
    public int Duration { get; set; }

    public void DealDamage()
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
public class Weakness : BaseStatus, IDurationStatus
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
}
public class Brittle : BaseStatus, IUseStatus
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
}
public class Rage : BaseStatus, IDurationStatus
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
        OutgoingDamageMultiplier = 1.5f;
    }   
    public float ApplyRage()
    {
        return OutgoingDamageMultiplier;
    }
}
public class Passion : BaseStatus, IDurationStatus
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
}
public class Poison : BaseStatus, IStackableStatus
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
}

public class ExchangeEffect : BaseStatus, IPartyStatus
{
    ManaType type;

    public ExchangeEffect(Sprite sprite, List<BaseCharacter> party, ManaType type)
    {
        Icon = sprite;
        this.type = type;
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
        switch (type)
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
}

