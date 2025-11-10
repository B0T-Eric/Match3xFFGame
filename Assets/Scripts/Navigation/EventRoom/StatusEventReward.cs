using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// base event reward is what each event will give either good or bad.
/// </summary>
[CreateAssetMenu(fileName = "StatusEventReward", menuName = "ScriptableObjects/Events/StatusReward")]
public class ExchangeReward : BaseEventReward, IRewardStatus
{
    [SerializeField]
    ExchangeEffect status;
    [SerializeField]
    ManaType type;
    
    private void OnEnable()
    {
        status = new ExchangeEffect(rewardSprite,type);
    }
    public BaseStatus GetReward()
    {
        return status;
    }
}
public interface IRewardStatus
{
    BaseStatus GetReward();
}