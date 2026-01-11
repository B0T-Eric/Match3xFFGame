using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// base event class:
/// Events should contain a set of options with rewards paired up to them,
/// 
/// </summary>
[Serializable]
public abstract class BaseEvent : MonoBehaviour
{
    public List<BaseEventReward> OptionOutcomes;
    [SerializeField]
    private string description;
    [SerializeField]
    private string eventName;
    protected List<Button> eventButtons = new List<Button>();
    [SerializeField]
    protected GameObject buttonPrefab;
    protected RectTransform buttonParentTransform;
    protected Button continueButton;
    protected TextMeshProUGUI titleText, descriptionText;
    // Start is called before the first frame update
    void Start()
    {
        InitializeRewards();
        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
        buttonParentTransform = GameObject.Find("ButtonPanel").GetComponent<RectTransform>();
        CreateOptions();
        titleText.text = eventName;
        descriptionText.text = description;
        continueButton.interactable = false;
    }
    protected virtual void CreateOptions()
    {
        foreach (Button b in eventButtons)
        {
            Destroy(b.gameObject);
        }
        eventButtons.Clear();
        for (int i = 0; i < OptionOutcomes.Count; i++)
        {
            GameObject eventOptionObj = Instantiate(buttonPrefab, this.transform, false);
            Button b = eventOptionObj.GetComponent<Button>();
            TextMeshProUGUI text = eventOptionObj.GetComponentInChildren<TextMeshProUGUI>();
            eventButtons.Add(b);
            int capturedIndex = i;
            text.text = $"{i + 1} - {OptionOutcomes[i].OptionSelectText}";
            b.onClick.AddListener(() => OnEventOptionSelected(capturedIndex));
            RectTransform rect = eventOptionObj.GetComponent<RectTransform>();
            rect.SetParent(buttonParentTransform);
        }
    }
    protected virtual  void InitializeRewards()
    {

    }
    //pulls reward from list with index, returning it to the on click method.
    protected virtual BaseEventReward GetReward(int index)
    {
        BaseEventReward reward = OptionOutcomes[index];
        if (reward != null) 
        {
            return reward;
        }  
        else
            Debug.Log(index + " is missing");
            return null;
    }
    
    protected virtual void OnEventOptionSelected(int index)
    {
        BaseEventReward reward = GetReward(index);
        if (reward == null) //if object is null cancel otherwise give reward.
        {
            Debug.Log("Cancelled or Reward was missing??");
        }
        else if(reward is IRewardStatus statusReward)
        {
            Party.Instance.AddStatus(statusReward.GetReward());
            GameResources.Instance.ExchangeRewardPool.RemoveAt(index);
        }
        ToggleButtons();
    }
    public void ToggleButtons()
    {
        continueButton.interactable = true;
        foreach (Button b in eventButtons)
        {
            b.gameObject.SetActive(false);
        }
    }
}
