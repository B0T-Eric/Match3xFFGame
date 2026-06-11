using System.Collections;
using UnityEngine;
public class DmgEnhance : Enhancement, IOnGetDamage
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
}
