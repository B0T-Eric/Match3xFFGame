using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public Button selectButton;
    public GameObject characterPrefab;
    private Color notSelectedColour = new Color(1f, 1f, 1f, 0.7f);
    private Color selectedColour = new Color(0f,1f,0.3f,0.7f);
    private Image panel;
    public CharacterSelectManager characterSelectManager;

    private bool isSelected = false;
    private void Start()
    {
        selectButton.onClick.AddListener(SelectButtonClicked);
        panel = GetComponent<Image>();
    }
    private void SelectButtonClicked()
    {
        isSelected = !isSelected;
        panel.color = isSelected ? selectedColour : notSelectedColour;
        characterSelectManager.OnCharacterSelected(characterPrefab, isSelected);
    }
}
