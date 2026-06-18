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
        foreach(BaseCharacter character in targets)
        {
            character.stats.onGetDmgFlat += GetDamage;
        }
    }
    public override void RemoveEnhancementEffect()
    {
        foreach (BaseCharacter character in targets)
        {
            character.stats.onGetDmgFlat -= GetDamage;
        }
    }

    public DmgEnhance(Sprite sprite, ManaType type, int damage) : base(sprite,type)
    {
        SetDamage(damage);
    }
}
