using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UserInterface
{
    public class ItemToolTip : ToolTip
    {
        DraggableItem item;
        private void Start()
        {
            item = GetComponent<DraggableItem>();
            Tooltip = $"<color=grey><b>{item.itemSO.itemName}</color></b>" +
                $"\n{item.GetDescription()}";
        }


    }
}