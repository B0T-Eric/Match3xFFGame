using Assets.Scripts.UserInterface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rewardText;
    [SerializeField]
    Image rewardSprite;
    public EndCombatScreen endCombatScreen;
    GameObject item;
    public Button button;
    public bool selected = false;
    public void Init(EndCombatScreen endCombatScreen)
    {
        this.endCombatScreen = endCombatScreen;
        button = GetComponent<Button>();
        //pick Item from List
        item = GetItem();
        rewardSprite.sprite = item.GetComponent<BaseItem>().itemSO.itemImage;
        rewardText.text = item.GetComponent<BaseItem>().itemSO.itemName;
        button.onClick.AddListener(() => endCombatScreen.SetSelectedItem(item));
        button.onClick.AddListener(() => endCombatScreen.ChangeButtonVisual(this));
        
    }

    public void UpdateVisuals()
    {
        if (selected)
        {
            Image image = GetComponent<Image>();
            Color color = Color.green;
            image.color = color;
        }
        else
        {
            Image image = GetComponent<Image>();
            Color color = Color.white;
            image.color = color;
        }
    }
    private GameObject GetItem()
    {
        ItemRarity rarity = GetRandomRarity();
        List<GameObject> validItems = GetItemsOfRarity(rarity);
        int rng = UnityEngine.Random.Range(0,validItems.Count);
        return validItems[rng];
    }

    private ItemRarity GetRandomRarity()
    {
        int rng = UnityEngine.Random.Range(0, 101);
        if (rng < 51)
        {
            return ItemRarity.Common;
        }
        else if(rng > 50 && rng < 76)
        {
            return ItemRarity.Uncommon;
        }
        else
        {
            return ItemRarity.Rare;
        }
    }

    private List<GameObject> GetItemsOfRarity(ItemRarity rarity)
    {
        GameResources instance = GameResources.Instance;
        switch (rarity) 
        {
            case ItemRarity.Common: return instance.CommonItemPool;
            case ItemRarity.Uncommon: return instance.UncommonItemPool;
            case ItemRarity.Rare: return instance.RareItemPool;
            default: return instance.ItemPool;
        }

        
    }
}
