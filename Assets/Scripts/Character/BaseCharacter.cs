using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { Attack, Heal, Retain}

public delegate void OnAttackDelegate(BaseCharacter character);
public delegate void OnDamageTakenDelegate(BaseCharacter baseCharacter);
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
    public ObservableCollection<BaseStatus> statusEffects;
    public int secPassiveThreshold = 20;
    public int thirdPassiveThreshold = 35;
    int num = 0;
    #endregion
    #region Character States
    public CharacterState state;
    public TurnState turnState;
    public Button abilityButton, turnStateButton;
    bool AbilityActive = false;
    #endregion
    #region Display
    public string hexColour;
    public GameObject partyUi;
    public Sprite sprite;
    internal InGameCharacterDisplay display;
    public ManaBar manaBar;
    public TextMeshProUGUI damageText;
    public CharacterInventorySlot[] inventorySlots;
    #endregion
    #region Character Inventory
    public List<DraggableItem> items;
    protected OnAttackDelegate onAttack;
    protected OnDamageTakenDelegate onDamageTaken;
    protected OnTurnEndDelegate onTurnEnd;
    protected OnTurnStartDelegate onTurnStart;
    #endregion
    public virtual void Init()
    {
        statusEffects = new ObservableCollection<BaseStatus>();
        currentMana = statsSO.initialMana;
        maxMana = statsSO.maxMana;
        state = CharacterState.Alive;
        potential = 0;
        nextTurnPotential = 0;
        items = new List<DraggableItem>();
        inventorySlots = new CharacterInventorySlot[3];
        statusEffects.CollectionChanged += StatusEffectsChanged;
        GameObject g = transform.GetChild(0).gameObject;  
        manaBar = g.GetComponent<ManaBar>();
        damageText = g.GetComponentInChildren<TextMeshProUGUI>();
        turnStateButton = g.GetComponentInChildren<Button>();
        inventorySlots[0] = GameObject.Find("ItemSlotOne").GetComponent<CharacterInventorySlot>();
        inventorySlots[1] = GameObject.Find("ItemSlotTwo").GetComponent<CharacterInventorySlot>();
        inventorySlots[2] = GameObject.Find("ItemSlotThree").GetComponent<CharacterInventorySlot>();
        turnStateButton.onClick.AddListener(ChangeTurnState);
        manaBar.Init(this);
        UpdateDisplays();
    }

    private void StatusEffectsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (display != null)
        {
            display.DisplayStatus(statusEffects);
        }
    }

    protected virtual void UpdateDisplays()
    {
        ChangeStateDisplay();
        DisplayPotential();
        manaBar.UpdateHealthBar(this);
    }
    //update sprite between alive, down and dead.
    private void ChangeStateDisplay()
    {
        if (state == CharacterState.Down) 
        {

        }
        else if (state == CharacterState.Dead) 
        {

        }
        else 
        {
            
        }

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
            //delegate for apply on hit effects/extra damage
            if(onAttack != null)
            {
                onAttack.Invoke(c);
            }
            
        }
        nextTurnPotential = 0;

    }
    protected virtual int GetDamage()
    {
        int damage = potential;
        //status based damage amp / conditional damage from abilities or other party members
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.flatDamageIncrease > 0)
            {
                damage += item.itemSO.flatDamageIncrease;
            }
            if (item.itemSO.damageMult > 0)
            {
                damage = (int)(damage * item.itemSO.damageMult);
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
            if(onDamageTaken != null)
            {
                foreach (OnDamageTakenDelegate onDamageTaken in onDamageTaken.GetInvocationList())
                {
                    onDamageTaken.Invoke(this);
                }
            }
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
        if(onTurnStart != null)
        {
            foreach (OnTurnStartDelegate onTurnStart in onTurnStart.GetInvocationList())
            {
                onTurnStart.Invoke();
            }
        }
        
        CheckStatusDurations();
        UpdateDisplays();
    }
    public virtual void TurnEnd()
    {
        if (onTurnEnd != null)
        {
            foreach (OnTurnEndDelegate onTurnEnd in onTurnEnd.GetInvocationList())
            {
                onTurnEnd.Invoke();
            } 
        }
        CheckStatusDurations();
    }
    //find an remove all status's with 0 or less duration/stacks since they're no longer effective
    private void CheckStatusDurations()
    {
        List<int> indexs = new List<int>();
        foreach (BaseStatus status in statusEffects)
        {
            if (status is IDurationStatus)
            {
                IDurationStatus s = (IDurationStatus)status;
                if (s.Duration <= 0)
                {
                    indexs.Add(statusEffects.IndexOf(status));
                }
            }
            if (status is IStackableStatus)
            {
                IStackableStatus s = (IStackableStatus)status;
                if(s.StackCount <= 0)
                {
                    indexs.Add(statusEffects.IndexOf(status));
                }
            }
        }
        for (int i = 0; i < indexs.Count; i++)
        {
            statusEffects.RemoveAt(indexs[i]);
        }
    }
    //calculate retain amount after items / other effects
    protected virtual float GetRetainMult()
    {
        return 0.75f;
    }
    public void ChangeTurnState()
    {
        
        if(num == 2)
        {
            num = 0;
        }
        else
        {
            num++;
        }
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
        statusEffects.Add(status);
        if(status is IOnTurnEnd)
        {
            IOnTurnEnd end = (IOnTurnEnd)status;
            onTurnEnd += end.OnTurnEnd;
        }
        if(status is IOnTurnStart)
        {
            IOnTurnStart start = (IOnTurnStart)status;
            onTurnStart += start.OnTurnStart;
        }
    }
    //remove a status effected from list on character
    public virtual void RemoveStatusEffect(int index)
    {
        BaseStatus status = statusEffects[index];
        if (status is IOnTurnEnd)
        {
            IOnTurnEnd end = (IOnTurnEnd)status;
            onTurnEnd -= end.OnTurnEnd;
        }
        if (status is IOnTurnStart)
        {
            IOnTurnStart start = (IOnTurnStart)status;
            onTurnStart -= start.OnTurnStart;
        }
        statusEffects.RemoveAt(index);
    }

    public void EquipItem(DraggableItem item)
    {
        items.Add(item);
        if (item is IOnAttackItem)
        {
            IOnAttackItem onAttackItem = (IOnAttackItem)item;
            onAttack += onAttackItem.OnAttack;
        }
        if (item is IOnDamageTakenItem)
        {
            IOnDamageTakenItem onDamageTakenItem = (IOnDamageTakenItem)item;
            onDamageTaken += onDamageTakenItem.OnDamageTaken;
        }
        if (item is IOnTurnEnd)
        {
            IOnTurnEnd onTurnEndItem = (IOnTurnEnd)item;
            onTurnEnd += onTurnEndItem.OnTurnEnd;
        }
        if (item is IOnTurnStart)
        {
            IOnTurnStart onTurnStartItem = (IOnTurnStart)item;
            onTurnStart += onTurnStartItem.OnTurnStart;
        }
    }
    public void UnequipItem(DraggableItem item)
    {
        items.Remove(item);
        if (item is IOnAttackItem)
        {
            IOnAttackItem onAttackItem = (IOnAttackItem)item;
            onAttack -= onAttackItem.OnAttack;
        }
        if (item is IOnDamageTakenItem)
        {
            IOnDamageTakenItem onDamageTakenItem = (IOnDamageTakenItem)item;
            onDamageTaken -= onDamageTakenItem.OnDamageTaken;
        }
        if (item is IOnTurnEnd)
        {
            IOnTurnEnd onTurnEndItem = (IOnTurnEnd)item;
            onTurnEnd -= onTurnEndItem.OnTurnEnd;
        }
        if (item is IOnTurnStart)
        {
            IOnTurnStart onTurnStartItem = (IOnTurnStart)item;
            onTurnStart -= onTurnStartItem.OnTurnStart;
        }
    }

}
