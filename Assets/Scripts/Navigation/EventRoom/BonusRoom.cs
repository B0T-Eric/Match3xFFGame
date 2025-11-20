using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRoom : MonoBehaviour
{
    private BaseEvent eventObj;
    private void Start()
    {
        Instantiate(GameResources.Instance.EventPool[Random.Range(0,GameResources.Instance.EventPool.Count)]);
    }
}
