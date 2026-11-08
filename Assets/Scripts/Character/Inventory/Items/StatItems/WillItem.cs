using System.Collections;
using UnityEngine;

public class WillItem : BaseItem, ICombinableItem
{

    public override string GetEffectDesc()
    {
        return $"Increase potential gained by {itemSO.potentialGainIncrease}";
    }
}
