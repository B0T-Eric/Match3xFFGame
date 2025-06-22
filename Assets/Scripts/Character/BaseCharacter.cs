using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { Attack, Heal, Retain}

public delegate void OnAttackDelegate(BaseCharacter character);
public delegate void OnDamageTakenDelegate();
public delegate void OnTurnEndDelegate();
public delegate void OnTurnStartDelegate();

[Serializable]
public abstract class BaseCharacter : MonoBehaviour
{
    #region Character Stat Values
    public CharacterStatsSO statsSO;
    public int currentMana;
    public int maxMana;
    public int potential;
    public int nextTurnPotential;
    public float retainMultiplier;
    public ManaType manaType;
    public List<BaseStatus> statusEffects;
    #endregion
    #region Character States
    public CharacterState state;
    public TurnState turnState;
    public Button abilityButton;
    bool AbilityActive = false;
    #endregion
    #region Display
    public ManaBar manaBar;
    public TextMeshProUGUI damageText, stateText;
    public CharacterInventorySlot[] inventorySlots;
    #endregion
     #region Character Inventory
    public List<DraggableItem> items;
    private OnAttackDelegate onAttack;
    private OnDamageTakenDelegate onDamageTaken;
    private OnTurnEndDelegate onTurnEnd;
    private OnTurnStartDelegate onTurnStart;
    #endregion
    public virtual void Init()
    {
        currentMana = statsSO.initialMana;
        maxMana = statsSO.maxMana;
        state = CharacterState.Alive;
        potential = 0;
        nextTurnPotential = 0;
        items = new List<DraggableItem>();
        inventorySlots = new CharacterInventorySlot[3];
        inventorySlots[0] = GameObject.Find("ItemSlotOne").GetComponent<CharacterInventorySlot>();
        inventorySlots[1] = GameObject.Find("ItemSlotTwo").GetComponent<CharacterInventorySlot>();
        inventorySlots[2] = GameObject.Find("ItemSlotThree").GetComponent<CharacterInventorySlot>();
        manaBar.Init(this);
        UpdateDisplays();
    }
    protected virtual void UpdateDisplays()
    {
        ChangeStateText();
        DisplayPotential();
        UpdateManaBar();
    }
    private void ChangeStateText()
    {
        stateText.text = $"State: {state.ToString()}";
    }
    private void UpdateManaBar()
    {
        manaBar.SetMana(currentMana);
        manaBar.SetManaText();
    }
    private void DisplayPotential()
    {
        switch (turnState)
        {
            case TurnState.Attack:
                damageText.color = Color.red;
                damageText.text = potential.ToString();
                break;
            case TurnState.Retain:
                damageText.color = Color.yellow;
                damageText.text = potential.ToString();
                break;
            case TurnState.Heal:
                damageText.color = Color.green;
                damageText.text = potential.ToString();
                break;
        }
    }
    public void ToggleAbility()
    {
        AbilityActive = !AbilityActive;
        if ( AbilityActive )
        {
            abilityButton.interactable = true;
        }
        else
        {
            abilityButton.interactable = false;
        }
    }
    public virtual void PreformTurn()
    {
        BaseCharacter target = null;
        if (this.GetType() != typeof(EnemyCharacter))
        {
            target = EnemyCharacter.Instance;
        }
        switch (turnState)
        {
            case TurnState.Attack: Attack(target); break;
            case TurnState.Heal: HealMana(potential); break;
            case TurnState.Retain: RetainPotential(); break;
        }
        UpdateDisplays();
    }
    public virtual void Attack(BaseCharacter c)
    {
        if(state == CharacterState.Alive)
        {
            int damage = GetDamage();
            c.LoseMana(damage);
            
        }
        nextTurnPotential = 0;

    }
    protected virtual int GetDamage()
    {
        int damage = potential;
        foreach(DraggableItem item in items)
        {
            if(item.itemSO.flatDamageIncrease > 0)
            {
                damage += item.itemSO.flatDamageIncrease;
            }
            if(item.itemSO.damageMult > 0)
            {
                damage = (int)(damage *item.itemSO.damageMult);
            }
        }
        return damage;
    }
    protected int GetDamageReduction()
    {
        int dmgReduction = 0;
        if(this is PurpleCharacter)
        {
            dmgReduction += 5;
        }
        foreach (BaseCharacter character in Party.Instance.members)
        {
            if (character is PurpleCharacter purp && !this)
            {
                dmgReduction += 1;
            }
        }
        foreach (DraggableItem item in items)
        {
            if(item.itemSO.damageReduction > 0)
            {
                dmgReduction += item.itemSO.damageReduction;
            }
        }
        return dmgReduction;
    }
    protected int GetHealingAmount()
    {
        int heal = potential;
        foreach(DraggableItem item in items)
        {
            if(item.itemSO.healingMult > 0)
            {
                heal = (int)(heal * item.itemSO.healingMult);
            }
        }
        return heal;
    }
    //character heal action (not ability related)
    public virtual void HealMana(int amount)
    {
        if(state != CharacterState.Dead)
        {
            currentMana += GetHealingAmount();
        }
        nextTurnPotential = 0;
    }
    //method for taking damage on character, is given positive amount which is then removed from mana pool.
    public virtual void LoseMana(int amount)
    {
        if(state != CharacterState.Dead && currentMana > 0)
        {
            amount -= GetDamageReduction();
            currentMana -= amount;
            if(currentMana <= 0  && state == CharacterState.Down)
            {
                currentMana = 0;
                state = CharacterState.Dead;
            }
            else if(currentMana <= 0 && state == CharacterState.Alive)
            {
                currentMana = 1;
                state = CharacterState.Down;
            }
        }
        UpdateDisplays();
    }
    public virtual float GetPotentialMult()
    {
        float mult = 1f;
        foreach (DraggableItem item in items) 
        {
            if (item.itemSO.potentialGainMult != 0f)
            {
                mult += item.itemSO.potentialGainMult;
            }
        }
        return mult;
    }
    public virtual int GetPotentialIncrease()
    {
        int flatIncrease = 0;
        foreach(DraggableItem item in items)
        {
            if(item.itemSO.potentialGainIncrease != 0)
            {
                flatIncrease += item.itemSO.potentialGainIncrease;
            }
        }
        return flatIncrease;
    }
    public virtual void GainPotential(int amount)
    {
        if (state != CharacterState.Dead || state != CharacterState.Down)
        {
            amount += GetPotentialIncrease();
            amount = (int)Math.Round(amount * GetPotentialMult());
            potential += amount;
        }
        UpdateDisplays();
    }
    
    public virtual void RetainPotential()
    {
        retainMultiplier = GetRetainMult();
        float potentialVal = potential * retainMultiplier;
        int rounded = Mathf.RoundToInt(potentialVal);
        nextTurnPotential = rounded;
    }
    //handle start of turn actions
    public virtual void TurnStart()
    {
        potential = nextTurnPotential;

        UpdateDisplays();
    }
    public virtual void TurnEnd()
    {
        List<BaseStatus> effects = new();
        foreach (BaseStatus status in statusEffects)
        {
            if(status.duration > 0)
            {
                status.duration--;
            }
            if(status.duration == 0)
            {
                effects.Add(status);
            }
        }
        foreach(BaseStatus status in effects)
        {
            RemoveStatusEffect(status);
        }
        foreach(OnTurnEndDelegate onTurnEnd in onTurnEnd.GetInvocationList())
        {
            onTurnEnd.Invoke();
        }
    }
    //calculate retain amount after items / other effects
    protected virtual float GetRetainMult()
    {
        return 0.75f;
    }
    public void ChangeTurnState(int num)
    {
        switch (num)
        {
            case 0: turnState = TurnState.Attack; break;
            case 1: turnState = TurnState.Retain; break;
            case 2: turnState = TurnState.Heal; break;
        }
        UpdateDisplays();
    }
    public void ResetTurnState() 
    {
        turnState = TurnState.Retain;
        UpdateDisplays();
    }

    //add a parsed in status effect to the list
    //first check if it exists to then either increase stack coun (if possible) and increase duration
    public virtual void AddStatusEffect(BaseStatus status)
    {
        if (statusEffects.Contains(status))
        {
            if (status.isStackable)
            {
                statusEffects[statusEffects.IndexOf(status)].stackCount++;
            }
            statusEffects[statusEffects.IndexOf(status)].duration = status.duration;
        }
        else
        {
            statusEffects.Add(status);
        }
            
    }
    //remove a status effected from list on character
    public virtual void RemoveStatusEffect(BaseStatus status)
    {
        statusEffects.Remove(status);
    }

    public void EquipItem(DraggableItem item)
    {
        items.Add(item);
        if (item.isOnAttack)
        {
            onAttack += item.OnAttack;
        }
        if (item.isOnDamageTaken)
        {
            onDamageTaken += item.OnDamageTaken;
        }
        if (item.isOnTurnEnd)
        {
            onTurnEnd += item.OnTurnEnd;
        }
        if (item.isOnTurnStart)
        {
            onTurnStart += item.OnTurnStart;
        }
    }
    public void UnequipItem(DraggableItem item)
    {
        items.Remove(item);
        if (item.isOnAttack)
        {
            onAttack -= item.OnAttack;
        }
        if (item.isOnDamageTaken)
        {
            onDamageTaken -= item.OnDamageTaken;
        }
        if (item.isOnTurnEnd)
        {
            onTurnEnd = item.OnTurnEnd;
        }
        if (item.isOnTurnStart)
        {
            onTurnStart -= item.OnTurnStart;
        }
    }
    
}
