using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventRoomManager : MonoBehaviour
{

    EventScriptableObject selectedEvent;
    public TextMeshProUGUI eventNameText, eventDescriptionText;
    [SerializeField]
    GameObject optionDisplayPrefab;
    [SerializeField]
    Transform optionDisplayParent;
    [SerializeField]
    Button continueButton;

    List<GameObject> options = new();
    // Start is called before the first frame update
    void Start()
    {
        continueButton.interactable = false;
        selectedEvent = GameResources.Instance.GetRandomEvent();
        DisplayEventInfo(selectedEvent);
    }
    void DisplayEventInfo(EventScriptableObject e)
    {
        eventNameText.text = e.eventName;
        eventDescriptionText.text = e.description;

        foreach (EventOption option in e.options)
        {

            GameObject g = Instantiate(optionDisplayPrefab, optionDisplayParent);
            EventOptionClick gOption = g.GetComponent<EventOptionClick>();
            gOption.Init(option, this);
            options.Add(g);
        }
    }

    public void OptionClick(int index)
    {
        switch (selectedEvent)
        {
            case SpiritExchangeEvent exchange:
                GetSpiritReward(exchange,index);
                break;
        }
        foreach (GameObject g in options)
        {
            Destroy(g);
        }
        options.Clear();
        continueButton.interactable = true;
    }
    void GetSpiritReward(SpiritExchangeEvent e,int index)
    {
        Party.Instance.statuses.Add(e.GetEffect(index));
    }
}
