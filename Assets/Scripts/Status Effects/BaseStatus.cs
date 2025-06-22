using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus : MonoBehaviour
{
    public int duration;
    public int stackCount;
    public bool isStackable;
    public int useCount;
}
