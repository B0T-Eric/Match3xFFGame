using System.Collections;
using UnityEngine;
public class MightItem : BaseItem
{

    public override string GetEffectDesc()
    {
        return $"Decrease damage taken by {itemSO.damageReduction}";
    }
}
