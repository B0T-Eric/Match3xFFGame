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

    private Image panel;
    private TextMeshProUGUI buttonText;

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

    }
}
