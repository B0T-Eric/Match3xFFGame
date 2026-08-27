using System.Collections;
using UnityEngine;

public class WillItem : BaseItem
{

    public override string GetEffectDesc()
    {
        return $"Increase potential gained by {itemSO.potentialGainIncrease}";
    }
}
