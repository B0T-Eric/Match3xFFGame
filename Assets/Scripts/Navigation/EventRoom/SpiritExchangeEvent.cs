using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "EventScriptableObject", menuName = "ScriptableObjects/Events/ExchangeSO")]
public class SpiritExchangeEvent : EventScriptableObject
{
    public Sprite[] effectSprites;
    Dictionary<int, ExchangeEffect> rewardEffects = new();
    public ExchangeEffect GetEffect(int i)
    {
        if (rewardEffects.Count == 0)
        {
            for (int j = 0; j < effectSprites.Length; j++)
            {
                rewardEffects.Add(j, new ExchangeEffect(effectSprites[j], Party.Instance.members, (ManaType)j + 1));
            }
        }
        ExchangeEffect effect;
        rewardEffects.TryGetValue(i, out effect);
        return effect;
    }

}
