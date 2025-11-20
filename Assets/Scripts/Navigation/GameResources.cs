using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;
    public static GameResources Instance {  get { return instance; } }
    public List<BaseEvent> EventPool = new List<BaseEvent>();
    public List<GameObject> ItemPool;
    public List<BaseStatus> PartyStatusPool;
    public List<ExchangeReward> ExchangeRewardPool;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }
    public GameObject GetRandomItem()
    {
        int rng = Random.Range(0, ItemPool.Count);
        GameObject item = ItemPool[rng];
        ItemPool.RemoveAt(rng);
        return item;
    }
    public BaseEvent GetRandomEvent()
    {
        if(EventPool == null || EventPool.Count == 0)
        {
            Debug.Log("EVENT POOL NULL OR EMPTY");
            return null;
        }
        else
        {
            int rng = Random.Range(0, EventPool.Count);
            BaseEvent e = EventPool[rng];
            EventPool.RemoveAt(rng);
            return e;
        }
    }
}
