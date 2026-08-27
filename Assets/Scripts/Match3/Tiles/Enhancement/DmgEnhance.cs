using System.Collections;
using UnityEngine;
public class DmgEnhance : Enhancement, IOnGetDamage, IOnClearRemove, IOnSpawnAdd
{
    private int Damage;

    public int GetDamage()
    {
        return Damage;
    }
    public void SetDamage(int damage)
    {
        Damage = damage;
    }
    public override void ApplyEnhancementEffect()
    {
        base.ApplyEnhancementEffect();
        foreach(BaseCharacter character in targets)
        {
            character.stats.onGetDmgFlat += GetDamage;
        }
    }
    public override void RemoveEnhancementEffect()
    {
        base.RemoveEnhancementEffect();
        foreach (BaseCharacter character in targets)
        {
            character.stats.onGetDmgFlat -= GetDamage;
        }
    }

    public DmgEnhance(Sprite sprite, ManaType type, string name, int damage) : base(sprite,type,name)
    {
        SetDamage(damage);
    }
}
