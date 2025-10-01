using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventOptionClick : MonoBehaviour
{
    EventOption option;
    TextMeshProUGUI textMeshPro;
    Button button;
    public void Init(EventOption option, EventRoomManager room)
    {  
        this.option = option;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = option.optionText;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => room.OptionClick(option.optionIndex));
    }
}
