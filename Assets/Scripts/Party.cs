using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public List<GameObject> selectedCharacters = new List<GameObject>();
    public List<Character> members = new List<Character>();
    public int maxPartySize = 4;
    public CharacterDisplayManager partyDisplayManager;
    public Button confirmButton;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
            foreach(GameObject character in selectedCharacters)
            {
                members.Add(character.GetComponent<Character>());
            }
            partyDisplayManager.DisplayParty();
            
            GameObject.Find("CharacterSelectCanvas").SetActive(false);
            
        }
    }
}
