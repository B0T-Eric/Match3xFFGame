using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;
    public static GameResources Instance {  get { return instance; } }
    [SerializeField]
    public List<BaseEvent> EventPool;
    [SerializeField]
    public List<GameObject> ItemPool;
    [SerializeField]
    public List<BaseStatus> PartyStatusPool;
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
        int rng = Random.Range(0, EventPool.Count);
        BaseEvent e = EventPool[rng];
        EventPool.RemoveAt(rng); 
        return e;
    }
}
