using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public RawImage image;
    public ItemScriptableObject item;
    public Button button;
    public InventoryPopup popup;
    private void Start()
    {
        button.onClick.AddListener(SelectItem);
    }
    private void SelectItem()
    {
        popup.selectedItem = item;
        popup.ViewItemDescription();
    }
}
