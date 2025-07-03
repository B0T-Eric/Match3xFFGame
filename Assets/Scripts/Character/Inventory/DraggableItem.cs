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
    //public bool isOnAttack;
    public bool isOnDamageTaken;
    public bool isOnTurnEnd;
    public bool isOnTurnStart;
    private void Start()
    {
        image.sprite = itemSO.itemImage;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        CharacterInventorySlot slot = transform.parent.GetComponent<CharacterInventorySlot>();
        if (slot != null) slot.UnequipItem();

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
        if(transform.GetComponent<CharacterInventorySlot>() != null)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
    public BaseStatus ApplyItemStatus()
    {
        return itemSO.status;
    }
}
public interface IOnAttackItem
{
    void OnAttack(BaseCharacter character);
}
public interface IOnDamageTakenItem
{
    void OnDamageTaken(BaseCharacter character);
}
public interface IOnTurnEnd
{
    void OnTurnEnd();
}
public interface IOnTurnStart
{
    void OnTurnStart();
}
public class EvilItem : DraggableItem
{
    //apply on attack to target character
    public virtual void OnAttack(BaseCharacter character)
    {
        character.statusEffects.Add(ApplyItemStatus());
    }
}

