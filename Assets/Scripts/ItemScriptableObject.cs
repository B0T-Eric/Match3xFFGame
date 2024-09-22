using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemScriptableObject : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public float health;
    public float damage;
    public ManaType type;
}
