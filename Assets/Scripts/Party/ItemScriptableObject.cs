using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemScriptableObject : ScriptableObject
{
    public int potentialGainIncrease;
    public float potentialGainMult;
    public float damageMult;
    public int flatDamageIncrease;
    public float healingMult;
    public int damageReduction;
    public int healFlat;
    public float retainMult;
    public BaseStatus status;
    public Sprite itemImage;
    public string itemName;
    public int ID;
    public ItemRarity Rarity;
}
