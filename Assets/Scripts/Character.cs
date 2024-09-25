using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    
    //damage 
    int turnDamage = 0;

    private void Start()
    {
        currentMana = statsSO.initialMana;
        party = gameObject.transform.root.GetComponent<Party>();
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
    private void FixedUpdate()
    {
        
    }
    //increase damage after every match by amount of tiles cleared of mana type
    public void IncreaseDamage(int amount)
    {
        //if there is an item of same mana type gain double damage
        if (HasMatchingManaType() && party.inventory.Count > 0)
        {
            turnDamage += ((2*(matchingItemCount))*amount);
            DisplayDamage();
        }
        else
        {
            turnDamage += amount;
            DisplayDamage();
        }
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
        }
        turnDamage = 0;
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
}
