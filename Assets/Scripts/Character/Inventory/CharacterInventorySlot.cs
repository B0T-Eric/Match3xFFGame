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
            draggableItem = dropped.GetComponent<DraggableItem>();
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
            draggableItem = null;
            character.UnequipItem(draggableItem);
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
