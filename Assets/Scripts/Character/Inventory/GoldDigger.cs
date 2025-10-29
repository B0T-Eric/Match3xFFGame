using System.Collections;
using TMPro;
using UnityEngine;

public class GoldDigger : DraggableItem, IStatItem
{
    [SerializeField]
    TextMeshProUGUI goldText;
    int goldCount = 0;
    bool IsUpgradedOnce, IsUpgradedTwice;
    public void GainGold()
    {
        int rng = Random.Range(15, 26);
        goldCount += rng;
        goldText.text = goldCount.ToString();
    }

    public void OnEquip()
    {
        if(IsUpgradedOnce && IsUpgradedTwice)
        {
            IncreaseStats(7);
        }
        else if (IsUpgradedOnce)
        {
            IncreaseStats(1);
        }
        //idk if it needs more..
    }

    public void OnUnequip()
    {
        if (IsUpgradedOnce && IsUpgradedTwice)
        {
            DecreaseStats(7);
        }
        else if (IsUpgradedOnce)
        {
            DecreaseStats(1);
        }
        //idk if it needs more..
    }

    public void TryUpgradeGoldDigger()
    {
        if (goldCount > 200 && !IsUpgradedOnce)
        {
            IsUpgradedOnce = true;
            IncreaseStats(1);
        }
        else if (goldCount > 1000 && IsUpgradedOnce && !IsUpgradedTwice)
        {
            IsUpgradedTwice = true;
            IncreaseStats(6);
        }
    }

    public void IncreaseStats(int num)
    {
        itemSO.potentialGainMult += num/10f;
        itemSO.potentialGainIncrease += num;
        itemSO.damageMult += num/10f;
        itemSO.damageReduction += num;
        itemSO.flatDamageIncrease += num;
        itemSO.healingMult += num/10f;
    }
    public void DecreaseStats(int num)
    {
        itemSO.potentialGainMult -= num / 10f;
        itemSO.potentialGainIncrease -= num;
        itemSO.damageMult -= num / 10f;
        itemSO.damageReduction -= num;
        itemSO.flatDamageIncrease -= num;
        itemSO.healingMult -= num / 10f;
    }
}