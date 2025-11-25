using System.Collections;
using TMPro;
using UnityEngine;

public class GoldDigger : DraggableItem, IStatItem
{
    [SerializeField]
    TextMeshProUGUI goldText;
    int goldCount = 0;
    bool IsUpgradedOnce, IsUpgradedTwice;
    int firstTier = 1;
    int tierIncrease = 6;
    int lastTier = 7;
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
            IncreaseStats(lastTier);
        }
        else if (IsUpgradedOnce)
        {
            IncreaseStats(firstTier);
        }
        //idk if it needs more..
    }

    public void OnUnequip()
    {
        if (IsUpgradedOnce && IsUpgradedTwice)
        {
            DecreaseStats(lastTier);
        }
        else if (IsUpgradedOnce)
        {
            DecreaseStats(firstTier);
        }
        //idk if it needs more..
    }

    public void TryUpgradeGoldDigger()
    {
        if (goldCount > 200 && !IsUpgradedOnce)
        {
            IsUpgradedOnce = true;
            IncreaseStats(firstTier);
        }
        else if (goldCount > 1000 && IsUpgradedOnce && !IsUpgradedTwice)
        {
            IsUpgradedTwice = true;
            IncreaseStats(tierIncrease);
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
    public override string GetEffectDesc()
    {
        return $"<b>Current Gold: {goldCount}</b>\nEvery time you complete a stage, gain a random amount of gold (15-25)." +
            $"\nAt 200 gold the item increases it's current stats to +{firstTier} and +{firstTier * 10}% respectively." +
            $"\nAt > 700 gold the item reaches it's peak stats amplifying them by +{tierIncrease} and +{tierIncrease * 10}% respectively." +
            $"\nFor a combined total of {lastTier} and {lastTier * 10}% to the respective stats.";
    }
}