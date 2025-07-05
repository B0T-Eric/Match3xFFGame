using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    private static Party _instance;
    public static Party Instance { get { return _instance; } }

    public List<GameObject> selectedCharacters = new List<GameObject>();
    public List<BaseCharacter> members = new List<BaseCharacter>();
    public int maxPartySize = 4;
    public int inventorySize = 7;
    public CharacterDisplayManager partyDisplayManager;
    public InventoryDisplayManager inventoryDisplayManager;
    public GameObject inventoryDisplay;
    public Button confirmButton;
    public InventoryPopup inventoryPopup;
    public bool HasKnowledgeOfEnemy;
    public int RestCount;

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
        if(confirmButton != null)
        {
            ChangeConfirmButtonState();
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
    }
}
