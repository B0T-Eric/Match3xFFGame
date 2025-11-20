using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpiritEvent : BaseEvent
{
    protected override  void InitializeRewards()
    {
        OptionOutcomes =  GameResources.Instance.ExchangeRewardPool.Cast<BaseEventReward>().ToList();
    }
    protected override BaseEventReward GetReward(int index)
    {
        BaseEventReward reward = OptionOutcomes[index];
        ExchangeReward exReward = reward as ExchangeReward;
        if(exReward.rewardSprite != null)
        {
            OptionOutcomes.RemoveAt(index);
            return reward;
        }
        else
        {
            return null;
        }
    }
}
