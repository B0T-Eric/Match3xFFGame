using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "UpgradeEventReward", menuName = "ScriptableObjects/Events/UpgradeReward")]
public class UpgradeEventReward : BaseEventReward
{
    public bool IsRandom;
    private bool isRandom;
    public int UpgradeUses;
    private int upgradeUses;
    private void OnEnable()
    {
        isRandom = IsRandom;
        upgradeUses = UpgradeUses;
    }
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
        if(upgradeUses == 0)
        {
            parent.ToggleButtons();
        }
        else
        {
            item.Upgrade();
            foreach (BaseCharacter c in Party.Instance.members)
            {
                //private modifiable upgrade uses minus the max/starting upgrade uses + base damage = resulting damage taken.
                c.LoseMana(damage + (upgradeUses - UpgradeUses));
            }
            upgradeUses--;
            Debug.Log($"Successful upgrade of {item}");
        }
        
    }
}


