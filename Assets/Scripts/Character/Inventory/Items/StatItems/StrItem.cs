using System.Collections;
using UnityEngine;

public class StrItem : BaseItem, ICombinableItem
{

    public override string GetEffectDesc()
    {
        return $"Increase damage by {itemSO.flatDamageIncrease}";
    }
}
