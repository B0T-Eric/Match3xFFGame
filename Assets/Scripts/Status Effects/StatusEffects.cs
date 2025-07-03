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
            base.character.currentMana -= Damage;
        }
    }
    public override void PreformEffect()
    {
        DealDamage();
        DecreaseDuration();
    }
    public void DecreaseDuration()
    {
        Duration--;
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
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
    }

    public void IncreaseDuration(int duration)
    {
        Duration += duration;
    }
    public float OutgoingDamageModifier;
    public Weakness(int duration, float modifier, Sprite sprite)
    {
        Icon = sprite;
        Duration = duration;
        OutgoingDamageModifier = modifier;
    }
}
public class Brittle : BaseStatus, IUseStatus
{
    public int UseCount { get; set; }

    public void DecreaseUseCount()
    {
        UseCount--;
    }

    public void IncreaseUseCount(int count)
    {
        UseCount += count;
    }
    public float IncomingDamageMultiplier;
    public Brittle(int useCount, float mult, Sprite sprite)
    {
        Icon = sprite;
        UseCount = useCount;
        IncomingDamageMultiplier = mult;
    }
}
public class Rage : BaseStatus, IDurationStatus
{
    public int Duration { get; set; }

    public void DecreaseDuration()
    {
        Duration--;
    }

    public void IncreaseDuration(int duration)
    {
        Duration++;
    }
    public float OutgoingDamageMultiplier;
    public Rage(Sprite sprite)
    {
        Icon=sprite;
        Duration = 1;
        OutgoingDamageMultiplier = 1.5f;
    }
}
public class Passion : BaseStatus, IDurationStatus
{
    public int Duration { get; set; }

    public void DecreaseDuration()
    {
        Duration--;
    }

    public void IncreaseDuration(int duration)
    {
        Duration++;
    }
    public float PotentialGainMult;
    public Passion(Sprite sprite)
    {
        Icon = sprite;
        Duration = 1;
        PotentialGainMult = 2f;
    }
}

