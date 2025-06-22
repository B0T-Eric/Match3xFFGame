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
        for (int i = 0; i < renderers.Count; i++)
        {
            if (i < items.Count)
            {
                renderers[i].texture = items[i].itemImage.texture;
            }
            else
            {
                renderers[i].texture = null;
            }
        }
    }
}
