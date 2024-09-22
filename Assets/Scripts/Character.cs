using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    //mana is the resource required for using abilities, and determines characters who are alive or dead
    public float currentMana;
    CharacterState state;
    public CharacterStatsSO statsSO;
    [SerializeField]
    public GameObject prefab;
    public ManaBar manaBar;
    private Party party;

    //damage 
    float turnDamage = 0;

    private void Start()
    {
        currentMana = statsSO.initialMana;
        party = gameObject.transform.root.GetComponent<Party>();
    }
    public void LoseMana(float amount)
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
    public void GainMana(float amount)
    {
        Debug.Log($" current mana = {currentMana} ") ;
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
        Debug.Log($" updated current mana = {currentMana} ");
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
        turnDamage += amount;
    }
    public void Attack()
    {
        //if there is an item of same mana type deal double damage
        if (HasMatchingManaType() && party.inventory.Count > 0)
        {
            turnDamage = turnDamage * 2;
            EnemyBehaviour.Instance.TakeDamage(turnDamage);
        }
        else
        {
            EnemyBehaviour.Instance.TakeDamage(turnDamage);
        }
        turnDamage = 0;
        
    }
    private bool HasMatchingManaType()
    {
        
        foreach (ItemScriptableObject item in party.inventory)
        {
            if (item.manaType == statsSO.type)
            {
                return true;
            }
        }
        return false;
    }
}
