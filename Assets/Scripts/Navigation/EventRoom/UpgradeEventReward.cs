using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "UpgradeEventReward", menuName = "ScriptableObjects/Events/UpgradeReward")]
public class UpgradeEventReward : BaseEventReward
{
    public bool IsRandom;
    public int UpgradeUses;
    public void UpgradeRandomItems(List<IUpgradableItem> items, int damage)
    {
        foreach (IUpgradableItem item in items)
        {
            item.Upgrade();
        }
        foreach (BaseCharacter c in Party.Instance.members)
        {
            c.LoseMana(damage);
        }
    }
    public void UpgradeItem(IUpgradableItem item, int damage, BaseEvent parent)
    {
        if(UpgradeUses == 0)
        {
            parent.ToggleButtons();
        }
        else
        {
            item.Upgrade();
            int copyUses = UpgradeUses;
            UpgradeUses--;
            foreach (BaseCharacter c in Party.Instance.members)
            {
                c.LoseMana(damage + (copyUses - UpgradeUses));
            }
            Debug.Log($"Successful upgrade of {item}");
        }
        
    }
}


