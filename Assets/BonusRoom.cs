using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRoom : MonoBehaviour
{
    private void Start()
    {
        Instantiate(GameResources.Instance.GetRandomEvent());
    }
}
