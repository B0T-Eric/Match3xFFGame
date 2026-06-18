using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    private static Party _instance;
    public static Party Instance { get { return _instance; } }
    public List<TileState> AllowedTileTypes { get; private set; } = new();
    public List<GameObject> selectedCharacters = new List<GameObject>();
    public List<BaseCharacter> members = new List<BaseCharacter>();
    public ObservableCollection<BaseStatus> statuses = new ObservableCollection<BaseStatus>();
    
    public int maxPartySize = 4;
    public int inventorySize = 7;
    public CharacterDisplayManager partyDisplayManager;
    public GameObject inventoryDisplay;
    public Button confirmButton;
    public StatusDisplayManager statusManager;
    public bool HasKnowledgeOfEnemy;
    public int RestCount;
    public TextMeshProUGUI StageText, TurnText, MovesText, StageNameText;
    public List<Enhancement> AvailableEnhancements = new List<Enhancement>();
    private void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        ToggleText();
        AllowedTileTypes = new List<TileState>
        {
            TileState.A,
            TileState.B,
            TileState.C,
            TileState.D,
            TileState.E,
            TileState.F,
            TileState.G
        };
        if (confirmButton != null)
        {
            ChangeConfirmButtonState();
        }
        statuses.CollectionChanged += Statuses_CollectionChanged;
    }


    public void RemoveTileType(TileState type)
    {
        AllowedTileTypes.Remove(type);
    }
    public void ToggleText()
    {
        StageText.gameObject.SetActive(!StageText.gameObject.activeSelf);
        TurnText.gameObject.SetActive(!TurnText.gameObject.activeSelf);
        MovesText.gameObject.SetActive(!MovesText.gameObject.activeSelf);
        StageNameText.gameObject.SetActive(!StageNameText.gameObject.activeSelf);
    }

    private void Statuses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (BaseStatus newStatus in e.NewItems)
            {
                statusManager.DisplayStatus(newStatus);
                newStatus.PreformEffect();
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (BaseStatus oldStatus in e.OldItems)
            {
                statusManager.RemoveDisplayedStatus(oldStatus);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            statusManager.RemoveAllDisplays();
        }
    }

    private void FixedUpdate()
    {
        if (confirmButton != null)
        {
            ChangeConfirmButtonState();
        }
    }
    private void ChangeConfirmButtonState()
    {
        confirmButton.interactable = selectedCharacters.Count > 0;
    }

    public void AddToSelectedCharacters(GameObject character)
    {
        if(selectedCharacters.Count < maxPartySize && !selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            
        }
    }
    public void RemoveFromSelectedCharacters(GameObject character)
    {
        selectedCharacters.Remove(character);
    }
    public void ConfirmPartySelection()
    {
        if(selectedCharacters.Count > 0)
        {
            partyDisplayManager.DisplayParty();
            GameObject.Find("CharacterSelectCanvas").SetActive(false);
            //inventoryDisplay.SetActive(true);
        }
    }
    public void IncreasePotential(ManaType type, int amount)
    {
        foreach(BaseCharacter character in members)
        {
            if(character.manaType == type)
            {
                character.GainPotential(amount);
            }
        }
    }
    public void DisplayCharactersOnGameStart()
    {
        GameContainer.Instance.DisplayParty(members);
        ResetPartyDamageTaken();
        foreach(BaseCharacter character in members)
        {
            character.CombatStart();
        }
    }
    //set damage taken to 0 on start of game
    private void ResetPartyDamageTaken()
    {
        foreach(BaseCharacter character in members)
        {
            character.ResetDamageTaken();
        }
    }
    public void EquipItemOnCharacter(BaseItem item)
    {
        foreach(BaseCharacter character in members)
        {
            if(character.stats.Items.Count < character.inventorySlots.Length)
            {
                character.EquipItem(item);
                break;
            }
        }
    }
    public void AddStatus(BaseStatus status)
    {
        statuses.Add(status);
    }
}
