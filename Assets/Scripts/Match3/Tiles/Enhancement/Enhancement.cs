using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enhancement
{
    protected Sprite sprite;
    protected ManaType type;
    protected List<BaseCharacter> targets;
    protected string name;
    protected string desc;
    public void SetName(string s)
    {
        name = s;
    }
    public string GetName()
    {
        return name;
    }
    public ManaType GetManaType()
    {
        return type;
    }
    public void SetType(ManaType type)
    {
        this.type = type;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }
    private string ListTargets()
    {
        string s = "";
        if (targets.Count == 1)
        {
            s = targets[0].GetCharacterName();
        }
        else
        {
            foreach (BaseCharacter c in targets)
            {
                s += $"{c.GetCharacterName()},";
            }
            char[] trimChars = { ',', ' ' };
            s = s.Trim(trimChars);
        }
        return s;
    }
    public virtual void ApplyEnhancementEffect()
    {
        CombatLog.Instance.AmendTextToLog($"Applied {name} enhancements effect to {ListTargets()}.");
    }
    public virtual void RemoveEnhancementEffect()
    {
        CombatLog.Instance.AmendTextToLog($"Removed {name} enhancements effect to {ListTargets()}.");
    }
    public void SetTargets(List<BaseCharacter> characters)
    {
        foreach(BaseCharacter c in characters)
        {
            //if the enhancement has a specific type then set
            if (c.manaType == type && type != ManaType.None)
            {
                targets.Add(c);
            }
            else if (type == ManaType.None)
            {
                targets = characters;
            }
        }
    }
    public Enhancement(Sprite sprite,ManaType type,string name)
    {
        SetSprite(sprite);
        SetType(type);
        SetTargets(Party.Instance.members);
        SetName(name);
    }
}
public interface IOnClearAdd
{

}
public interface IOnSpawnAdd
{

}
public interface IOnSpawnRemove
{

}
public interface IOnClearRemove
{

}

public static class EnhancementHandler
{
    /// <summary>
    /// base chance to apply enhancement.
    /// </summary>
    private static int chance = 15;
    /// <summary>
    /// collection containing enhancements, first in first out so the enhancements are cycled through.
    /// </summary>
    private static Queue<Enhancement> Enhancements = new Queue<Enhancement>();
    public static List<Enhancement> FetchAllEnhancements()
    {
        return Enhancements.ToArray().ToList();
    }
    public static void AddEnhancement(Enhancement enhancement)
    {
        Enhancements.Enqueue(enhancement);
    }
    private static Enhancement GetEnhancement()
    {
        Enhancement e = Enhancements.Dequeue();
        Match3Skin.Instance.enhanceQueueDisplay.UpdateDisplay();
        return e;
    }
    /// <summary>
    /// Randomly return an enhancement, with escalating chance.
    /// </summary>
    /// <returns>Null on fail to land the dice roll.
    /// </returns>Enhancement from front of queue.
    public static Enhancement TryGetEnhancement()
    {
        if(Enhancements.Count > 0)
        {
            if (chance <= Random.Range(0, 101))
            {
                chance = 15;
                return GetEnhancement();
            }
            else
            {
                chance += 5;
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    public static Enhancement[] enhancePool = new Enhancement[]
        {
            new DmgEnhance(GameResources.Instance.enhancementSprites[0],ManaType.None,"Damage",1),

        };
}