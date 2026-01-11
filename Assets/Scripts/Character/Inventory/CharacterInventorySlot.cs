using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public BaseCharacter character;
    public bool isOccupied = false;
    DraggableItem draggableItem;
    public DraggableItem GetItem()
    {
        return draggableItem;
    }
    public void DisplayEquipedItem(GameObject itemObject)
    {
        GameObject obj = Instantiate(itemObject);
        obj.transform.SetParent(this.transform);
        DraggableItem item = obj.GetComponent<DraggableItem>();
        draggableItem = item;
        character.EquipItem(item);
        isOccupied = true;
    }
    private void Start()
    {
        character = transform.parent.parent.GetComponentInParent<BaseCharacter>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 && !isOccupied)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped == null) return;
            var item = dropped.GetComponent<DraggableItem>();
            if (item == null) return;
            draggableItem = item;
            draggableItem.parentAfterDrag = this.transform;
            character.EquipItem(draggableItem);
            isOccupied = true;
        }
    }
    public void UnequipItem()
    {
        if (isOccupied)
        {
            isOccupied = false;
            character.UnequipItem(draggableItem);
            draggableItem = null;
        }
    }
    public void EquipItem(DraggableItem item)
    {
        draggableItem = item;
        if (!isOccupied)
        { 
            isOccupied = true;
            character.EquipItem(draggableItem);
        }
    }
}
