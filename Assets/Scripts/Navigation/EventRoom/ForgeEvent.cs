using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgeEvent : BaseEvent
{
    List<IUpgradableItem> allPlayerItems = new();

    [SerializeField]
    int randomDamage, choiceBaseDamage;
    protected override void InitializeRewards()
    {
        OptionOutcomes = GameResources.Instance.ForgeRewardPool.Cast<BaseEventReward>().ToList();
        foreach (BaseCharacter c in Party.Instance.members)
        {
            foreach (DraggableItem item in c.items)
            {
                if(item is IUpgradableItem uItem)
                allPlayerItems.Add(uItem);
            }
        }
    }
    protected override void OnEventOptionSelected(int index)
    {
        BaseEventReward reward = GetReward(index);
        if (reward is UpgradeEventReward upReward)
        {
            if (upReward.IsRandom) //if the reward is random, pick items randomly for upgrade.
            {
                List<IUpgradableItem> itemsToUpgrade = GetItemsToUpgrade(upReward);
                upReward.UpgradeRandomItems(itemsToUpgrade, randomDamage);
                ToggleButtons();
            }
            else //if not let player pick.
            {
                DisplayUpgradeOptions(upReward);
            }
        }
        
    }
    private void DisplayUpgradeOptions(UpgradeEventReward upReward)
    {
        foreach (Button b in eventButtons)
        {
            Destroy(b.gameObject);
        }
        eventButtons.Clear();
        for (int i = 0; i <= allPlayerItems.Count; i++)
        {
            GameObject eventOptionObj = Instantiate(buttonPrefab, this.transform, false);
            Button b = eventOptionObj.GetComponent<Button>();
            TextMeshProUGUI text = eventOptionObj.GetComponentInChildren<TextMeshProUGUI>();
            eventButtons.Add(b);
            int capturedIndex = i;
            if(capturedIndex == allPlayerItems.Count)
            {
                //cancel button
                text.text = "Cancel Upgrades.";
                b.onClick.AddListener(CreateOptions);
                RectTransform rect = eventOptionObj.GetComponent<RectTransform>();
                rect.SetParent(buttonParentTransform);
            } 
            else
            {
                DraggableItem item = allPlayerItems[i] as DraggableItem;
                IUpgradableItem upItem = item as IUpgradableItem;
                text.text = $"Upgrade {item.itemSO.itemName}";
                b.onClick.AddListener(() => upReward.UpgradeItem(upItem,choiceBaseDamage,this));
                RectTransform rect = eventOptionObj.GetComponent<RectTransform>();
                rect.SetParent(buttonParentTransform);
            }
            
        }
    }
    private List<IUpgradableItem> GetItemsToUpgrade(UpgradeEventReward upReward)
    {
        List<IUpgradableItem> items = new List<IUpgradableItem>();
        List<IUpgradableItem> copy = new List<IUpgradableItem>(allPlayerItems);
        int amount = Mathf.Min(upReward.UpgradeUses, copy.Count);
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, copy.Count);
            items.Add(copy[index]);
            copy.RemoveAt(index);
        }
        return items;

    }
}
