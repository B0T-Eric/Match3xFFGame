using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rewardText;
    [SerializeField]
    RawImage rewardSprite;
    [SerializeField]
    List<GameObject> items; 

    GameObject selectedItem;
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
        GameObject.Find("GameContainer").SetActive(false);
        //pick Item from List
        selectedItem = GetItem();
        rewardSprite.texture = selectedItem.GetComponent<DraggableItem>().itemSO.itemImage.texture;
        rewardText.text = selectedItem.GetComponent<DraggableItem>().itemSO.itemName;
        button.onClick.AddListener(EquipItem);
    }

    private void EquipItem()
    {
        BaseCharacter character = null;
        foreach(BaseCharacter c in Party.Instance.members)
        {
            if(c.items.Count < 3)
            {
                character = c;
                break;
            }
        }
        if(character != null)
        {
            foreach (CharacterInventorySlot slot in character.inventorySlots)
            {
                if (slot.isOccupied) continue;
                else slot.DisplayEquipedItem(selectedItem); break;
            }
        }
        
        gameObject.SetActive(false);
        continueButton.enabled =true;
    }

    private GameObject GetItem()
    {
        int itemIndex = Random.Range(0, items.Count);
        return items[itemIndex];
    }

}
