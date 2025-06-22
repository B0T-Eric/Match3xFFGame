using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public Button selectButton;
    public GameObject characterPrefab;
    private Color notSelectedColour = new Color(1f, 1f, 1f, 0.39f);
    private Color selectedColour = new Color(0f,1f,0.3f,0.39f);
    private Image panel;
    private TextMeshProUGUI buttonText;
    string addText = "Add To Party";
    string removeText = "Remove";
    public CharacterSelectManager characterSelectManager;

    private bool isSelected = false;
    private void Start()
    {
        buttonText = selectButton.GetComponentInChildren<TextMeshProUGUI>();
        selectButton.onClick.AddListener(SelectButtonClicked);
        panel = GetComponent<Image>();
    }
    private void SelectButtonClicked()
    {
        isSelected = !isSelected;
        buttonText.text = isSelected ? removeText : addText;
        panel.color = isSelected ? selectedColour : notSelectedColour;
        characterSelectManager.OnCharacterSelected(characterPrefab, isSelected);
    }
}
