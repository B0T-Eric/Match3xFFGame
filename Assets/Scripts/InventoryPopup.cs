using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    public List<ItemScriptableObject> items;
    public ItemScriptableObject selectedItem;
    [SerializeField]
    List<InventorySlot> slots;
    [SerializeField]
    GameObject descriptionPanel;
    TextMeshProUGUI descriptionText;
    Button removeItemButton;
    

    private void Start()
    {
        items = Party.Instance.inventory;
        removeItemButton = descriptionPanel.GetComponentInChildren<Button>();
        removeItemButton.onClick.AddListener(RemoveSelectedItem);
        descriptionText = descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
        descriptionPanel.gameObject.SetActive(false);
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].item = items[i];
            slots[i].image.texture = items[i].itemImage.texture;
        }
    }
    private void RemoveSelectedItem()
    {
        Party.Instance.RemoveItem(selectedItem);
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
    }
}
