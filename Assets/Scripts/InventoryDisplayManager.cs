using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayManager : MonoBehaviour
{
    [SerializeField]
    List<RawImage> renderers;
    [SerializeField]
    List<TextMeshProUGUI> textBoxes;
    public void DisplayItems(List<ItemScriptableObject> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            renderers[i].texture = items[i].itemImage.texture;
            textBoxes[i].text = items[i].count.ToString();
        }
    }
}
