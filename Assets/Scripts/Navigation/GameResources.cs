using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;
    public static GameResources Instance {  get { return instance; } }
    public List<BaseEvent> EventPool;
    public List<GameObject> ItemPool;
    public List<GameObject> CommonItemPool;
    public List<GameObject> UncommonItemPool;
    public List<GameObject> RareItemPool;
    public List<BaseStatus> PartyStatusPool;
    public List<ExchangeReward> ExchangeRewardPool;
    public List<UpgradeEventReward> ForgeRewardPool;
    public List<CompanionEventReward> CompanionRewardPool;
    //recipees dictionary (id of item 1, id of item 2, id of result item)
    public Dictionary<(int,int),int> MergeItemRecipes = new();
    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        AddRecipes();
    }
    public virtual void AddRecipes()
    {
        //poison catalyst and plague = plague explosion
        MergeItemRecipes.Add((3, 9), 10);
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
