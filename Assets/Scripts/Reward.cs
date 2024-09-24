using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rewardText;
    [SerializeField]
    SpriteRenderer rewardSprite;
    [SerializeField]
    List<ItemScriptableObject> items;
    ItemScriptableObject selectedItem;
    private Button button;
    [SerializeField]
    public Match3Skin match3;
    [SerializeField]
    
    private void Start()
    {
        button = GetComponent<Button>();
        //pick Item from List
        selectedItem = GetItem();
        rewardSprite.sprite = selectedItem.itemImage;
        rewardText.text = selectedItem.itemName;
        button.onClick.AddListener(EquipItem);
    }

    private void EquipItem()
    {
        Party.Instance.AddItem(selectedItem);
        Destroy(gameObject);
    }

    private ItemScriptableObject GetItem()
    {
        int itemIndex = Random.Range(0, items.Count);
        return items[itemIndex];
    }

}
