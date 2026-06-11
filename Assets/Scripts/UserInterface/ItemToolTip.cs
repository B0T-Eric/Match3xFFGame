using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UserInterface
{
    public class ItemToolTip : ToolTip
    {
        BaseItem item;
        private void Start()
        {
            item = GetComponent<BaseItem>();
            
        }
        private void Update()
        {
            Tooltip = $"<color=grey><b>{item.itemSO.itemName}</color></b>" +
                $"\n{item.GetDescription()}";
        }

    }
}