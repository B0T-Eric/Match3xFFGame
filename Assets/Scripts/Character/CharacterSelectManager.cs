using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public List<Button> buttons;
    [SerializeField]
    private Image[] selectedIndicators;
    public Button confirmButton;
    public GameObject charSelectCanvas;
    public GameObject startCanvas;
    [SerializeField]
    private GameObject[] charPrefabs;
    private GameObject selectedCharacter;
    private bool isCharacterInParty = false;
    #region Description
    [SerializeField]
    private TextMeshProUGUI nameText, descText;
    [SerializeField]
    private Image[] passiveIcons;
    [SerializeField]
    private Button[] passiveButtons;
    [SerializeField]
    private Image charImage;
    [SerializeField]
    private Button addButton;
    [SerializeField] 
    private TextMeshProUGUI addButtonText;
    string addText = "Add To Party";
    string removeText = "Remove From Party";
    private Color notSelectedColour = new Color(1f, 1f, 1f, 1f);
    private Color selectedColour = new Color(0f, 1f, 0.3f, 0.39f);
    #endregion
    #region Preview
    //The movable party for rearranging order of party.
    [SerializeField]
    ImageDrop[] previewSlots;
    [SerializeField]
    private GameObject previewItem;
    private Dictionary<int,GameObject> items = new();
    #endregion
    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            if(index == 0)
            {
                DisplayCharacterDetails(index);
            }
            buttons[i].onClick.AddListener(() => DisplayCharacterDetails(index));
        }
        Party.Instance.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(RearrangeAndAdd);
    }
    void DisplayCharacterDetails(int index)
    {
        if (selectedCharacter != null)
        {
            Destroy(selectedCharacter);
        }
        isCharacterInParty = IsCharacterInParty(index);
        selectedCharacter = Instantiate(charPrefabs[index]);
        BaseCharacter character = selectedCharacter.GetComponent<BaseCharacter>();
        nameText.text = character.GetCharacterName();
        charImage.sprite = character.sprite;
        for(int i = 0; i < passiveIcons.Length;i++)
        {
            passiveIcons[i].sprite = character.characterDescription.passiveIcons[i];
        }
        for(int i = 0;i < passiveButtons.Length; i++)
        {
            passiveButtons[i].onClick.RemoveAllListeners();
            int y = i;
            passiveButtons[i].onClick.AddListener(() => DisplayPassiveDescription(y, character));
        }
        addButton.onClick.RemoveAllListeners();
        addButton.onClick.AddListener(() => OnAddButtonClick(index,character.sprite));
        DisplayPassiveDescription(0, character);
        ChangeAddButton(index);
    }
    void OnAddButtonClick(int index, Sprite sprite)
    {
        if (!isCharacterInParty)
        {
            DisplayCharacterInPreview(sprite,index);
        }
        else
        {
            RemoveCharacterFromPreview(index);
        }
        isCharacterInParty = !isCharacterInParty;
        ChangeAddButton(index);
    }
    bool IsCharacterInParty(int index)
    {
        return items.ContainsKey(index);
    }
    void ChangeAddButton(int index)
    {
        addButtonText.text = isCharacterInParty ? removeText : addText;
        selectedIndicators[index].color = isCharacterInParty ? selectedColour : notSelectedColour;
        UpdateButtonEnabled();
    }
    void DisplayCharacterInPreview(Sprite sprite,int index)
    {
        ImageDrop slot = null;
        foreach(ImageDrop drop in previewSlots)
        {
            if(drop.Occupant == null)
            {
                slot = drop;
                break;
            }
            else
            {
                continue;
            }
        }
        RectTransform slotRect = slot.gameObject.GetComponent<RectTransform>();
        GameObject previewObj = Instantiate(previewItem, slotRect);
        DraggableImage drag = previewObj.GetComponent<DraggableImage>();
        drag.charIndex = index;
        slot.Place(drag);
        Image image = previewObj.GetComponent<Image>();
        image.sprite = sprite;
        items.Add(index,previewObj);
    }
    void RemoveCharacterFromPreview(int index)
    {
        GameObject objToDestroy;
        items.TryGetValue(index, out objToDestroy);
        Destroy(objToDestroy);
        items.Remove(index);
    }
    void RearrangeAndAdd()
    {
        List<GameObject> selectedCharacters = new();
        foreach(ImageDrop slot in previewSlots)
        {
            if(slot.Occupant != null)
            selectedCharacters.Add(charPrefabs[slot.Occupant.charIndex]);
        }
        //add to party.
        Party.Instance.gameObject.SetActive(true);
        Party.Instance.selectedCharacters = selectedCharacters;
        Party.Instance.ConfirmPartySelection();
        startCanvas.SetActive(true);
        HoverToolTip.HideTootTip_Static();
    }
    void DisplayPassiveDescription(int index, BaseCharacter character)
    {
        switch (index)
        {
            case 0: descText.text = $"{character.GetMatchDesc()}"; break;
            case 1: descText.text = $"{character.GetPassiveOneDesc()}"; break;
            case 2: descText.text = $"{character.GetPassiveTwoDesc()}"; break;
            case 3: descText.text = $"{character.GetPassiveThreeDesc()}"; break;
        }
    }
    void UpdateButtonEnabled()
    {
        bool isPartyFull = items.Count >= Party.Instance.maxPartySize;
        if (isCharacterInParty && isPartyFull || !isPartyFull)
        {
            addButton.interactable = true;
        }
        else
        {
            addButton.interactable = false;
        }
        if(items.Count > 0)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }
}
