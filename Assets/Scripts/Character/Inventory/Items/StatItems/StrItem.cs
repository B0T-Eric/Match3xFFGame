using System.Collections;
using UnityEngine;

public class StrItem : BaseItem
{

    public override string GetEffectDesc()
    {
        return $"Increase damage by {itemSO.flatDamageIncrease}";
    }
}
