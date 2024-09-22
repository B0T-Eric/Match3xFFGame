using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public List<Button> buttons;
    public Button confirmButton;
    private Party partyManager;

    public GameObject charSelectCanvas;
    public GameObject startCanvas;
    private void Start()
    {
        partyManager = FindAnyObjectByType<Party>();
        if(buttons != null)
        {
            foreach (Button button in buttons)
            {
                SelectionController controller = button.GetComponentInParent<SelectionController>();
                if (controller == null)
                {
                    Debug.Log("Controller is missing");
                }
                controller.characterSelectManager = this;
            }
        }
        else
        {
            Debug.Log("Buttons are null");
        }
        
        
        confirmButton.onClick.AddListener(partyManager.ConfirmPartySelection);
        confirmButton.onClick.AddListener(ToggleCanvas);
    }
    void ToggleCanvas()
    {
        charSelectCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }
    internal void OnCharacterSelected(GameObject characterPrefab, bool isSelected)
    {
        if(isSelected)
        {
            partyManager.selectedCharacters.Add(characterPrefab);
        }
        else
        {
            partyManager.selectedCharacters.Remove(characterPrefab);
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool isPartyFull = partyManager.selectedCharacters.Count >= partyManager.maxPartySize;
        foreach (Button button in buttons)
        {
            
            SelectionController controller = button.GetComponentInParent<SelectionController>();
            if ((controller == null))
            {
                Debug.Log("controller not found");
            }
            button.interactable = !isPartyFull || partyManager.selectedCharacters.Contains(controller.characterPrefab);
        }
    }

    
}
