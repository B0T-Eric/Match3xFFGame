
using UnityEngine;
using UnityEngine.EventSystems;


public class CharacterInventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public BaseCharacter character;
    public bool isOccupied = false;
    DraggableItem equippedItem;
    public DraggableItem GetItem()
    {
        return equippedItem;
    }
    private void Start()
    {
        character = transform.parent.parent.GetComponentInParent<BaseCharacter>();
    }
    public void DisplayEquipedItem(GameObject itemObject)
    {
        GameObject obj = Instantiate(itemObject);
        obj.transform.SetParent(this.transform);
        DraggableItem item = obj.GetComponent<DraggableItem>();
        equippedItem = item;
        character.EquipItem(item);
        isOccupied = true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;
        DraggableItem incomingItem = dropped.GetComponent<DraggableItem>();
        if (incomingItem == null) return;
        if (!isOccupied)
        {   
            equippedItem = incomingItem;
            equippedItem.parentAfterDrag = this.transform;
            character.EquipItem(equippedItem);
            isOccupied = true;

        }
        else if(isOccupied)
        {
            if (equippedItem is ICombinableItem && incomingItem is ICombinableItem)
            {
                GameObject combinedItem = DraggableItem.LookUpRecipe(equippedItem.itemSO.ID, incomingItem.itemSO.ID);
                GameObject itemToDestroy = equippedItem.gameObject;
                UnequipItem();
                Destroy(incomingItem.gameObject);
                Destroy(itemToDestroy);
                DisplayEquipedItem(combinedItem);
            }
            else
            {
                return;
            }
        }
    }
    public void UnequipItem()
    {
        if (isOccupied)
        {
            isOccupied = false;
            character.UnequipItem(equippedItem);
            equippedItem = null;
        }
    }
    public void EquipItem(DraggableItem item)
    {
        equippedItem = item;
        if (!isOccupied)
        { 
            isOccupied = true;
            character.EquipItem(equippedItem);
        }
    }
}
