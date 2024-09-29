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
    RawImage rewardSprite;
    [SerializeField]
    List<ItemScriptableObject> items;

    ItemScriptableObject selectedItem;
    private Button button;

    [SerializeField]
    Button continueButton;


    [SerializeField]
    Canvas canvas;
    private void Start()
    {
        continueButton.enabled = false;
        canvas = GetComponentInParent<Canvas>();
        button = GetComponent<Button>();
        
        //pick Item from List
        selectedItem = GetItem();
        rewardSprite.texture = selectedItem.itemImage.texture;
        rewardText.text = selectedItem.itemName;
        button.onClick.AddListener(EquipItem);
    }

    private void EquipItem()
    {
        Party.Instance.AddItem(selectedItem);
        gameObject.SetActive(false);
        continueButton.enabled =true;
    }

    private ItemScriptableObject GetItem()
    {
        int itemIndex = Random.Range(0, items.Count);
        return items[itemIndex];
    }

}
