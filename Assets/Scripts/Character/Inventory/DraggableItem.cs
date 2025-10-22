using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static DraggableItem;

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
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        
        CharacterInventorySlot newSlot = parentAfterDrag.GetComponentInParent<CharacterInventorySlot>();
        if (newSlot != null && newSlot != startSlot)
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
    public BaseStatus ApplyItemStatus()
    {
        return itemSO.status;
    }
}
// item effects to trigger on attack (triggered on the hit)
public interface IOnAttackItem
{
    void OnAttack(BaseCharacter character);
}
// item effects to trigger on damage taken
public interface IOnDamageTakenItem
{
    void OnDamageTaken(BaseCharacter character);
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


