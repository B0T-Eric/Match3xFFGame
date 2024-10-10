using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    //mana is the resource required for using abilities, and determines characters who are alive or dead
    public int currentMana;
    public CharacterState state;
    public CharacterStatsSO statsSO;
    [SerializeField]
    public GameObject prefab;
    public ManaBar manaBar;
    public TextMeshProUGUI damageText, stateText;
    private int matchingItemCount = 0;
    private Party party;
    [SerializeField]
    public Button abilityButton;
    //Ability states
    public bool regenActive = false;
    bool burnActive = false;
    bool doubleDamage = false;
    bool lifesteal = false;
    //regen amount
    int regenAmount = 5;
    //how many times regen should proc
    public int regenCount = 0;
    //damage 
    int turnDamage = 0;

    private void Start()
    {
        currentMana = statsSO.initialMana;
        party = gameObject.transform.root.GetComponent<Party>();
        abilityButton.onClick.AddListener(ActivateAbility);
        abilityButton.interactable = false;
    }
    public void LoseMana(int amount)
    {
        if(state != CharacterState.Dead)
        {
            if (state == CharacterState.Alive)
            {
                if (currentMana - amount <= 0 || currentMana - amount == 1)
                {
                    state = CharacterState.Down;
                    currentMana = 1;
                    UpdateManaBar();
                }
                else
                {
                    currentMana -= amount;
                    UpdateManaBar();
                }
            }
            else if (state == CharacterState.Down)
            {
                if (currentMana - amount < 0)
                {
                    state = CharacterState.Dead;
                    currentMana = 0;
                    UpdateManaBar();
                }
            }
        }      
    }
    private void Update()
    {
        ChangeStateText();
    }

    private void ChangeStateText()
    {
        stateText.text = $"State: {state.ToString()}";
    }

    public void GainMana(int amount)
    {
        if (state != CharacterState.Dead)
        {
            if (state != CharacterState.Down)
            {
                if (currentMana + amount > statsSO.maxMana)
                {
                    currentMana = statsSO.maxMana;
                    UpdateManaBar();
                }
                else
                {
                    currentMana += amount;
                    UpdateManaBar();
                }
            }
            else
            {
                currentMana += amount;
                state = CharacterState.Alive;
                UpdateManaBar();
            }
        }      
    }
    private void UpdateManaBar()
    {
        manaBar.SetMana(currentMana);
        manaBar.SetManaText();
    }
    //increase damage after every match by amount of tiles cleared of mana type
    public void IncreaseDamage(int amount)
    {
        //if there is an item of same mana type gain double damage
        if (HasMatchingManaType() && party.inventory.Count > 0 && (state != CharacterState.Down || state != CharacterState.Dead))
        {
            turnDamage += ((2*(matchingItemCount))*amount);
            if (doubleDamage)
            {
                turnDamage = turnDamage * 2;
            }
            DisplayDamage();
        }
        else if(state != CharacterState.Down || state != CharacterState.Dead)
        {
            turnDamage += amount;
            if (doubleDamage)
            {
                turnDamage = turnDamage * 2;
            }
            DisplayDamage();
        }
    }
    public void Reset()
    {
        burnActive = false;
        doubleDamage = false;
        turnDamage = 0;
        DisplayDamage();
    }
    public void DisplayDamage() 
    {
        damageText.SetText($"{turnDamage}");
    }
    public void Attack()
    {
        if(state != CharacterState.Down || state != CharacterState.Dead)
        {
            EnemyBehaviour.Instance.TakeDamage(turnDamage);
            if (lifesteal)
            {
                GainMana(turnDamage);
            }
            if(burnActive)
            {
                EnemyBehaviour.Instance.isBurning = true;
                EnemyBehaviour.Instance.burnDamage = turnDamage * 2;
            }
        }
        turnDamage = 0;
        doubleDamage = false;
        burnActive = false;
        DisplayDamage();
    }
    private bool HasMatchingManaType()
    {
        
        foreach (ItemScriptableObject item in party.inventory)
        {
            if (item.manaType == statsSO.type)
            {
                matchingItemCount = item.count;
                return true;
            }
        }
        return false;
    }
    private void ActivateAbility()
    {
        switch(statsSO.type)
        {
            case ManaType.Yellow:
                if(currentMana > 10)
                {
                    LoseMana(10);
                    regenActive = true;
                    regenCount = 4;
                    party.RemoveItem(FindUltimateOrb());
                }
                
            break;
            case ManaType.Green:
                if(currentMana > 30)
                {
                    LoseMana(30);
                    foreach (Character character in party.members)
                    {
                        character.GainMana(20);
                    }
                    party.RemoveItem(FindUltimateOrb());
                }
                
            break;
            case ManaType.Blue:
                if(currentMana > 15)
                {
                    LoseMana(15);
                    turnDamage += 20;
                    party.RemoveItem(FindUltimateOrb());
                }
            break;
            case ManaType.Purple:
                if(currentMana > 15)
                {
                    LoseMana(15);
                    doubleDamage = true;
                    party.RemoveItem(FindUltimateOrb());
                }
            break;
            case ManaType.Pink:
                if (currentMana > 20)
                {
                    LoseMana(20);
                    lifesteal = true;
                    party.RemoveItem(FindUltimateOrb());
                }
            break;
            case ManaType.Orange:
                if(currentMana > 25)
                {
                    LoseMana(25);
                    burnActive = true;
                    party.RemoveItem(FindUltimateOrb());
                }
            break;
            case ManaType.White:
                if(currentMana > 10)
                {
                    LoseMana(10);
                    Attack();
                    party.RemoveItem(FindUltimateOrb());
                }
            break;
        }
    }

    private ItemScriptableObject FindUltimateOrb()
    {
        foreach(ItemScriptableObject item in party.inventory) 
        {
            if(item.itemName == "Ultimate Orb")
            {
                return item;
            }
        }
        return null;
    }
}
