using System.Collections;
using UnityEngine;

public class HopeItem : BaseItem
{
    public override string GetEffectDesc()
    {
        return $"Increase mana restoration by {itemSO.healFlat}";
    }
}
