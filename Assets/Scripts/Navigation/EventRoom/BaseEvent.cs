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
public class BaseEvent : MonoBehaviour
{
    public List<BaseEventReward> OptionOutcomes;
    [SerializeField]
    private string description;
    [SerializeField]
    private string eventName;
    List<Button> eventButtons = new List<Button>();
    [SerializeField]
    private GameObject buttonPrefab;
    private RectTransform buttonParentTransform;
    private Button continueButton;
    private TextMeshProUGUI titleText, descriptionText;
    // Start is called before the first frame update
    void Start()
    {
        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
        buttonParentTransform = GameObject.Find("ButtonPanel").GetComponent<RectTransform>();
        for (int i = 0; i < OptionOutcomes.Count; i++)
        {
            GameObject eventOptionObj = Instantiate(buttonPrefab, this.transform, false);
            Button b = eventOptionObj.GetComponent<Button>();
            TextMeshProUGUI text = eventOptionObj.GetComponentInChildren<TextMeshProUGUI>();
            eventButtons.Add(b);
            int capturedIndex = i;
            text.text = $"{i+1} - {GetReward(i).OptionSelectText}";
            b.onClick.AddListener(() => OnEventOptionSelected(capturedIndex));
            RectTransform rect = eventOptionObj.GetComponent<RectTransform>();
            rect.SetParent(buttonParentTransform);
        }
        titleText.text = eventName;
        descriptionText.text = description;
        continueButton.interactable = false;
    }
    protected BaseEventReward GetReward(int index)
    {
        BaseEventReward reward = OptionOutcomes[index];
        if (reward != null)
            return reward;
        else
            Debug.Log(index + " is missing");
            return null;
    }
    protected virtual void OnEventOptionSelected(int index)
    {
        BaseEventReward reward = GetReward(index);
        if (reward == null)
        {
            Debug.Log("Cancelled or Reward was missing??");
        }
        else if(reward is IRewardStatus statusReward)
        {
            Party.Instance.AddStatus(statusReward.GetReward());
        }
        continueButton.interactable = true;
    }
}
