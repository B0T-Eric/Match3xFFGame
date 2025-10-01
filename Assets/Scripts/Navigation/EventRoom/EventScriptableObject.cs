using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "EventScriptableObject", menuName = "ScriptableObjects/Events")]

public class EventScriptableObject : ScriptableObject
{
    [SerializeField]
    public List<EventOption> options = new List<EventOption>();
    public string eventName;
    [TextArea]
    public string description;
}

