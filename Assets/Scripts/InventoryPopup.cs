using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    public ItemScriptableObject selectedItem;
    [SerializeField]
    List<InventorySlot> slots;
    [SerializeField]
    GameObject descriptionPanel;
    TextMeshProUGUI descriptionText;
    Button removeItemButton;
    

    private void Start()
    {
        
        removeItemButton = descriptionPanel.GetComponentInChildren<Button>();
        removeItemButton.onClick.AddListener(RemoveSelectedItem);
        descriptionText = descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
        descriptionPanel.gameObject.SetActive(false);
        PopupDisplayItems();
    }

    public void PopupDisplayItems()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < Party.Instance.inventory.Count)
            {
                slots[i].item = Party.Instance.inventory[i];
                slots[i].image.texture = Party.Instance.inventory[i].itemImage.texture;

            }
            else
            {
                slots[i].item = null;
                slots[i].image.texture = null;
            }
        }
    }

    public void RemoveSelectedItem()
    {
        Party.Instance.RemoveItem(selectedItem);
        Party.Instance.inventoryDisplayManager.DisplayItems(Party.Instance.inventory);
        PopupDisplayItems();
        descriptionPanel.gameObject.SetActive(false);
    }

    public void ViewItemDescription()
    {
        if(selectedItem != null)
        {
            descriptionPanel.gameObject.SetActive(true);
            descriptionText.text = selectedItem.description;
        }
    }
    public void CloseDisplay()
    {
        gameObject.SetActive(false);
        selectedItem = null;
        descriptionPanel.gameObject.SetActive(false);
        descriptionText.text = "";
    }
}
