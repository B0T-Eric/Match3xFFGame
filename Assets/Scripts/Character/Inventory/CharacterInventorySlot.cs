using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInventorySlot : MonoBehaviour, IDropHandler
{
    private BaseCharacter character;
    public bool isOccupied = false;
    DraggableItem draggableItem;
    public void DisplayEquipedItem(GameObject itemObject)
    {
        GameObject obj = Instantiate(itemObject);
        obj.transform.SetParent(this.transform);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 && !isOccupied)
        {
            GameObject dropped = eventData.pointerDrag;
            draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = this.transform;
            character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
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
        }
        else
        {
            character.EquipItem(draggableItem);
        }
    }
}
