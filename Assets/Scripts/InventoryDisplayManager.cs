using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayManager : MonoBehaviour
{
    [SerializeField]
    List<RawImage> renderers;
    public void DisplayItems(List<ItemScriptableObject> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            renderers[i].texture = items[i].itemImage.texture;
        }
    }
}
