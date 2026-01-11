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
    RawImage rewardSprite;

    GameObject selectedItem;
    private Button button;

    [SerializeField]
    Button continueButton;
    [SerializeField]
    Button skipButton;

    [SerializeField]
    Canvas canvas;
    private void Start()
    {
        continueButton.enabled = false;
        continueButton.interactable = false;
        canvas = GetComponentInParent<Canvas>();
        button = GetComponent<Button>();
        GameObject.Find("GameContainer").SetActive(false);
        //pick Item from List
        selectedItem = GetItem();
        rewardSprite.texture = selectedItem.GetComponent<DraggableItem>().itemSO.itemImage.texture;
        rewardText.text = selectedItem.GetComponent<DraggableItem>().itemSO.itemName;
        button.onClick.AddListener(EquipItem);
    }

    private void EquipItem()
    { 
        foreach(BaseCharacter c in Party.Instance.members)
        {
            foreach (CharacterInventorySlot slot in c.inventorySlots)
            {
                bool isSlotFree = !slot.isOccupied && slot.transform.childCount == 0;
                if(isSlotFree)
                {
                    slot.DisplayEquipedItem(selectedItem);
                    gameObject.SetActive(false);
                    continueButton.enabled = true;
                    continueButton.interactable = true;
                    skipButton.gameObject.SetActive(false);
                    return;
                }       
            } 
        }
        Debug.Log("Didn't Find Empty Slot");
    }
    public void SkipItem()
    {
        gameObject.SetActive(false);
        continueButton.enabled = true;
        continueButton.interactable = true;
        skipButton.gameObject.SetActive(false);
    }
    private GameObject GetItem()
    {
        ItemRarity rarity = GetRandomRarity();
        List<GameObject> validItems = GetItemsOfRarity(rarity);
        //logic to prevent duplicates maybe ?
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
