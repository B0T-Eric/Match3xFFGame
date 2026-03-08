using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum ItemRarity { None,Common, Uncommon, Rare, Cursed}

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemScriptableObject itemSO;
    public Image image;
    public Transform parentAfterDrag;
    private CharacterInventorySlot startSlot;
    protected BaseCharacter itemHolder;
    private void Start()
    {
        itemHolder = transform.parent.parent.parent.parent.GetComponent<BaseCharacter>();
        image = GetComponent<Image>();
        image.sprite = itemSO.itemImage;
        
    }
    public string GetDescription()
    {
        return$"\n<b>Stats:</b>" +
            $"\n{GetItemStatDesc()}" +
            $"\n<b>Effects</b>" +
            $"\n{GetEffectDesc()}" +
            $"\n<b>Upgrades</b>" +
            $"\n{GetUpgradeEffectDesc()}";
    }
    public virtual string GetEffectDesc()
    {
        return "No Special Effects.";
    }
    public virtual string GetUpgradeEffectDesc()
    {
        return "Not Upgradable.";
    }
    public string GetItemStatDesc()
    {
        List<string> parts = new List<string>();
        if (itemSO.potentialGainIncrease > 0)
            parts.Add($"+{itemSO.potentialGainIncrease} Potential Gain");
        if (itemSO.potentialGainMult > 0)
            parts.Add($"+{itemSO.potentialGainMult * 100f}% Potential Gain");
        if (itemSO.damageMult > 0)
            parts.Add($"+{itemSO.damageMult * 100f}% Damage");
        if (itemSO.flatDamageIncrease > 0)
            parts.Add($"+{itemSO.flatDamageIncrease} Flat Damage");
        if (itemSO.healingMult > 0)
            parts.Add($"+{itemSO.healingMult * 100f}% Healing");
        if (itemSO.damageReduction > 0)
            parts.Add($"{itemSO.damageReduction}% Damage Reduction");
        if (parts.Count == 0)
            return "No stat bonuses.";

        return string.Join("\n", parts);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        CharacterInventorySlot slot = transform.parent.GetComponent<CharacterInventorySlot>();
        if (slot != null)
        {
            startSlot = slot;
            slot.UnequipItem();
        }
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        
        CharacterInventorySlot newSlot = parentAfterDrag.GetComponentInParent<CharacterInventorySlot>();
        if (newSlot == null)
            return;
        else
        {
            if (newSlot != startSlot)
            {
                transform.SetParent(newSlot.transform);
            }
            else
            {
                transform.SetParent(startSlot.transform);
                startSlot.EquipItem(this);
            }
            image.raycastTarget = true;
        }

    }
    public static GameObject LookUpRecipe(int id1, int id2)
    {
        int resultId = 0;
        if(GameResources.Instance.MergeItemRecipes.TryGetValue((id1,id2), out resultId))
        {
            return GameResources.Instance.ItemPool[resultId];
        }
        else if(GameResources.Instance.MergeItemRecipes.TryGetValue((id2, id1), out resultId))
        {
            return GameResources.Instance.ItemPool[resultId];
        }
        else
        {
            Debug.Log("Failed to find valid combination.");
            return null;
        }
    }
    
}
// item effects to trigger on attack (triggered on the hit)
public interface IOnAttackItem
{
    void OnAttack(BaseCharacter character, int damage);
}
// item effects to trigger on damage taken
public interface IOnDamageTakenItem
{
    void OnDamageTaken(BaseCharacter baseCharacter, int damage);
}
public interface IOnHitItem
{
    void OnHit(BaseCharacter character);
}
// item effects to trigger on turn end
public interface IOnTurnEnd
{
    void OnTurnEnd();
}
// item effects to trigger on turn start
public interface IOnTurnStart
{
    void OnTurnStart();
}
//items which can be upgraded.
public interface IUpgradableItem
{
    public int Tier { get; set; }
    public int UpgradeAmount { get; set; }
    public float UpgradeModifier { get; set; }
    void Upgrade();
}
public interface ICombinableItem 
{

}
