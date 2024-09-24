using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemScriptableObject : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public ManaType manaType;
    public string description;
    public int count;
    private void OnEnable()
    {
        count = 0;
    }
}
