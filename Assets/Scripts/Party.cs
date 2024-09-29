using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    private static Party _instance;
    public static Party Instance { get { return _instance; } }

    public List<GameObject> selectedCharacters = new List<GameObject>();
    public List<Character> members = new List<Character>();
    public int maxPartySize = 4;
    public int inventorySize = 7;
    public CharacterDisplayManager partyDisplayManager;
    public InventoryDisplayManager inventoryDisplayManager;
    public GameObject inventoryDisplay;
    public List<ItemScriptableObject> inventory;
    public Button confirmButton;
    public InventoryPopup inventoryPopup;
    private void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        if(confirmButton != null)
        {
            ChangeConfirmButtonState();
        }
    }
    private void FixedUpdate()
    {
        if (confirmButton != null)
        {
            ChangeConfirmButtonState();
        }
    }
    public void AddItem(ItemScriptableObject item)
    {
        if(inventory.Contains(item))
        {
            inventory[inventory.IndexOf(item)].count++;
        }
        else
        {
            inventory.Add(item);
            inventory[inventory.IndexOf(item)].count++;
            inventoryDisplayManager.DisplayItems(inventory);
            inventoryPopup.PopupDisplayItems();
        }
    }
    public void RemoveItem(ItemScriptableObject item)
    {
        if(item.count > 1)
        {
            item.count--;
        }
        else
        {
            inventory.Remove(item);
            inventoryDisplayManager.DisplayItems(inventory);
        }
    }

    private void ChangeConfirmButtonState()
    {
        confirmButton.interactable = selectedCharacters.Count > 0;
    }

    public void AddToSelectedCharacters(GameObject character)
    {
        if(selectedCharacters.Count < maxPartySize && !selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
        }
    }
    public void RemoveFromSelectedCharacters(GameObject character)
    {
        selectedCharacters.Remove(character);
    }
    public void ConfirmPartySelection()
    {
        if(selectedCharacters.Count > 0)
        {
            partyDisplayManager.DisplayParty();
            GameObject.Find("CharacterSelectCanvas").SetActive(false);
            inventoryDisplay.SetActive(true);
        }
    }
    public void IncreaseManaAndDamage(ManaType type, int amount)
    {
        foreach(Character character in members)
        {
            if(character.statsSO.type == type)
            {
                character.IncreaseDamage(amount);
                character.GainMana(amount);
            }
        }
    }
}
