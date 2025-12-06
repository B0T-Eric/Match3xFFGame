using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCompanionEvent : BaseEvent
{
    [SerializeField]
    int rewardIndex;
    int cancelIndex = 7;
    CompanionEventReward selectedCompanion;
    List<BaseCharacter> validCharacters; //the characters who can have a companion.
    //base event pulls 0 by default
    //this method should fetch the related reward from pool based on subclass type.
    protected override void InitializeRewards()
    {
        OptionOutcomes = new List<BaseEventReward>
        {
            GameResources.Instance.CompanionRewardPool[rewardIndex],
            GameResources.Instance.CompanionRewardPool[cancelIndex]
        };
        validCharacters = new List<BaseCharacter>();
        foreach (BaseCharacter character in Party.Instance.members)
        {
            if (character.companion == null) //if the character doesn't already have a companion.
            {
                validCharacters.Add(character);
            }
        }
    }
    protected override void OnEventOptionSelected(int index)
    {
        
        BaseEventReward reward = GetReward(index);
        if (reward == null)
        {
            Debug.Log("Cancelled or Invalid");
            ToggleButtons();
        }
        else
        {
            selectedCompanion = reward as CompanionEventReward;
            DisplayCharacterSelectOptions();
        }
        
    }
    private void DisplayCharacterSelectOptions()
    {
        foreach(Button b in eventButtons)
        {
            Destroy(b.gameObject);
        }
        eventButtons.Clear();
        for(int i = 0; i <= validCharacters.Count; i++)
        {
            GameObject charSelectObj = Instantiate(buttonPrefab, this.transform, false);
            Button b = charSelectObj.GetComponent<Button>();
            TextMeshProUGUI text = charSelectObj.GetComponentInChildren<TextMeshProUGUI>();
            eventButtons.Add(b);
            int index = i;
            if (i == validCharacters.Count)
            {
                //cancel button
                text.text = "Cancel Selection.";
                b.onClick.AddListener(CreateOptions);
                RectTransform rect = charSelectObj.GetComponent<RectTransform>();
                rect.SetParent(buttonParentTransform);
            }
            else
            {
                BaseCharacter character = validCharacters[i];
                text.text = $"Assign companion to {character.GetCharacterName()}";
                b.onClick.AddListener(() => selectedCompanion.InitializeCompanion(character, this));
                RectTransform rect = charSelectObj.GetComponent<RectTransform>();
                rect.SetParent(buttonParentTransform);
            }
        }
    }
}
