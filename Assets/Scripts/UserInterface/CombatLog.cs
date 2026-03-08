using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CombatLog : MonoBehaviour
{
    public static CombatLog Instance { get; private set; }
    [SerializeField]
    TextMeshProUGUI log, title;
    List<string> combatLogs = new();
    string logText;
    int currentLogIndex = 0;
    int turn = 1;
    Button nextButton, lastButton, previousButton, firstButton;
    private void Start()
    {
        Instance = this;
        InitializeButtons();
    }
    //add a line for every action that is sent into for this current turn
    //this should function independently from the displaying log.
    public void AmendTextToLog(string message)
    {
        logText += $"-{message}.\n";
        if(currentLogIndex == turn-1)
        {
            log.text = logText;
        }
    }
    //end of every turn save the string to the logs, so player can cycle through the log.
    //the logText is cleared to for the start of the next turn to begin.
    public void StoreTurnLog()
    {
        combatLogs[turn-1] = logText;
        logText = "";
        IncreaseTurn();
        CreateLogIndex();
        UpdateButtons();
    }
    public void IncreaseTurn()
    {
        turn++;
    }
    public void CreateLogIndex()
    {
        combatLogs.Add("");
    }
    public void SetIndex(int i)
    {
        currentLogIndex = i;
        UpdateLog();
    }
    public void ResetLog()
    {
        combatLogs.Clear();
        currentLogIndex = 0;
        logText = "";
        title.text = "";
        log.text = logText;
        turn = 1;
        CreateLogIndex();
    }
    public void UpdateLog()
    {
        title.text = $"Turn {currentLogIndex + 1}";
        log.text = combatLogs[currentLogIndex];
        UpdateButtons();
    }
    protected void InitializeButtons()
    {
        firstButton = GameObject.Find("First").GetComponent<Button>();
        previousButton = GameObject.Find("Previous").GetComponent<Button>();
        lastButton = GameObject.Find("Last").GetComponent<Button>();
        nextButton = GameObject.Find("Next").GetComponent<Button>();
        firstButton.onClick.AddListener(FirstClick);
        previousButton.onClick.AddListener(PreviosClick);
        lastButton.onClick.AddListener(LastClick);
        nextButton.onClick.AddListener(NextClick);
        
    }
    public void UpdateButtons()
    {
        if (combatLogs.Count == 0)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else if (currentLogIndex == 0 && combatLogs.Count == 1)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else if (currentLogIndex == 0)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = true;
            nextButton.interactable = true;
        }
        else if (currentLogIndex == combatLogs.Count - 1)
        {
            previousButton.interactable = true;
            firstButton.interactable = true;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
            firstButton.interactable = true;
            lastButton.interactable = true;
            nextButton.interactable = true;
        }
    }
    public void LastClick()
    {
        currentLogIndex = combatLogs.Count - 1;
        UpdateLog();
        UpdateButtons();
    }
    public void PreviosClick()
    {
        if (currentLogIndex > 0)
            currentLogIndex--;
        UpdateLog();
        UpdateButtons();
    }
    public void NextClick()
    {
        if (currentLogIndex < combatLogs.Count - 1)
            currentLogIndex++;
        UpdateLog();
        UpdateButtons();
    }
    public void FirstClick()
    {
        currentLogIndex = 0;
        UpdateLog();
        UpdateButtons();
    }
}
