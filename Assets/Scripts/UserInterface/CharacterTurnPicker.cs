using Assets.Scripts.UserInterface;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTurnPicker : MonoBehaviour
{
    [SerializeField]
    GameObject container,attacksContainer, attackPrefab;
    [SerializeField]
    Button heal, attack, defend, state;
    BaseCharacter character;
    [SerializeField]
    Sprite healSprite, attackSprite, defendSprite, noneSprite;
    Image buttonRenderer;
    List<GameObject> attackOptions = new();
    private void Start()
    {
        buttonRenderer = GetComponent<Image>();
        buttonRenderer.sprite = noneSprite;
        state.onClick.AddListener(StateClick);
        heal.onClick.AddListener(HealClick);
        attack.onClick.AddListener(AttackClick);
        defend.onClick.AddListener(DefendClick);
        ToggleContainer();
    }
    public void SetCharacter(BaseCharacter character)
    {
        this.character = character;
    }
    public void StateClick()
    {
        ToggleContainer();
    }
    private void UpdateSprite(Sprite sprite)
    {
        buttonRenderer.sprite = sprite;
    }
    private void ToggleContainer()
    {
        container.SetActive(!container.activeSelf);
        if (attacksContainer.activeSelf)
        {
            ToggleAttackContainer();
        }
        HoverToolTip.HideTootTip_Static();
    }
    public void HealClick()
    {
        character.SetTurnState(TurnState.Heal);
        UpdateSprite(healSprite);
        ToggleContainer();
    }
    private void ToggleAttackContainer()
    {
        attacksContainer.SetActive(!attacksContainer.activeSelf);
    }
    public void AttackClick()
    {
        character.SetTurnState(TurnState.Attack);
        UpdateSprite(attackSprite);
        ToggleAttackContainer();
        int i = 0;
        foreach(GameObject _object in attackOptions)
        {
            Destroy(_object);
        }
        attackOptions.Clear();
        foreach(BasicAttack attack in character.attacks)
        {
            GameObject attackOptionObj = Instantiate(attackPrefab, attacksContainer.transform, false);
            attackOptions.Add(attackOptionObj);
            AttackOption attackOption = attackOptionObj.GetComponent<AttackOption>();
            attackOption.Init(i,attack.Name,attack.Description,character);
            Button b = attackOptionObj.GetComponent<Button>();
            b.onClick.AddListener(ToggleContainer);
            i++;
        }
    }
    public void DefendClick()
    {
        character.SetTurnState(TurnState.Retain);
        UpdateSprite(defendSprite);
        ToggleContainer();
    }
}
