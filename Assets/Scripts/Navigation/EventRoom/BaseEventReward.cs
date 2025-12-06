using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// base event reward is what each event will give either good or bad.
/// </summary>

public abstract class BaseEventReward : ScriptableObject
{
    public Sprite rewardSprite;
    public string OptionSelectText;
}
