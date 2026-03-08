using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveCatalyst : MonoBehaviour
{
    public void TryEquipCatalyst()
    {
        foreach (BaseCharacter c in Party.Instance.members)
        {
            foreach (CharacterInventorySlot slot in c.inventorySlots)
            {
                bool isSlotFree = !slot.isOccupied && slot.transform.childCount == 0;
                if (isSlotFree)
                {
                    GameObject selectedItem = GameResources.Instance.ItemPool[3];
                    slot.DisplayEquipedItem(selectedItem);
                    return;
                }
            }
        }
        Debug.Log("Didn't Find Empty Slot");
    }
}
