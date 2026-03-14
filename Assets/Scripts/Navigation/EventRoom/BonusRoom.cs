
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BonusRoom : MonoBehaviour
{
    private List<BaseEvent> validPool;
    //the method should select and instantiate an event based on many factors.
    private void Start()
    {
        validPool = new(GameResources.Instance.EventPool);
        if (Party.Instance.statuses.OfType<ExchangeReward>().FirstOrDefault()) //Remove exchange events
        {
            validPool.RemoveAll(e => e is SpiritEvent);
        }
        if (AllCharactersHaveCompanions()) //remove companion events from the pool.
        {
            validPool.RemoveAll(e => e is BaseCompanionEvent);
        }
        Instantiate(validPool[Random.Range(0,validPool.Count)]);
    }
    private bool AllCharactersHaveCompanions()
    {
        foreach(BaseCharacter c in Party.Instance.members)
        {
            if(c.companion == null)
            {
                return false;
            }
        }
        return true;
    }
}
