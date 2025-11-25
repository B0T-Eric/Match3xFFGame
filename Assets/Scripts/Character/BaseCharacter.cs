using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { Attack, Heal, Retain}

//on attack params: BaseCharacter character = the targetted character by the attack (either enemy or party member)
public delegate void OnAttackDelegate(BaseCharacter character, int damage);
//on damage taken params: BaseCharacter baseCharacter = the character which has just recieved damage.
public delegate void OnDamageTakenDelegate(BaseCharacter baseCharacter);
//
public delegate void OnTurnEndDelegate();
//
public delegate void OnTurnStartDelegate();

public struct PassiveValues
{
    public int secPassiveThreshold;
    public int thirdPassiveThreshold;
    public PassiveValues(int s, int t)
    {
        secPassiveThreshold = s;
        thirdPassiveThreshold = t;
    }
}
[Serializable]
public abstract class BaseCharacter : MonoBehaviour
{
    int num = 0;
    #region Character Stat Values
    public CharacterStatsSO statsSO;
    public int currentMana;
    public int maxMana;
    public int potential;
    public int nextTurnPotential;
    public float retainMultiplier;
    public ManaType manaType;
    public ObservableCollection<BaseStatus> statusEffects;
    protected PassiveValues passiveValues;
    
    #endregion
    #region Character States
    public CharacterState state;
    public TurnState turnState;
    public Button abilityButton, turnStateButton;
    bool AbilityActive = false;
    #endregion
    #region Display
    public string hexColour;
    public string description;
    public GameObject partyUi;
    public Sprite sprite;
    internal InGameCharacterDisplay display;
    public ManaBar manaBar;
    public TextMeshProUGUI potentialText, healthText;
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
        
        passiveValues = new PassiveValues(20,35);
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
        var tmps = g.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in tmps)
        {
            if (tmp.gameObject.name.Contains("Potential"))
                potentialText = tmp;
            else if (tmp.gameObject.name.Contains("Health"))
                healthText = tmp;
        }
        turnStateButton = g.GetComponentInChildren<Button>();
        //inventorySlots[0] = GameObject.Find("ItemSlotOne").GetComponent<CharacterInventorySlot>();
        //inventorySlots[1] = GameObject.Find("ItemSlotTwo").GetComponent<CharacterInventorySlot>();
        //inventorySlots[2] = GameObject.Find("ItemSlotThree").GetComponent<CharacterInventorySlot>();
        inventorySlots = GetComponentsInChildren<CharacterInventorySlot>(true);
        turnStateButton.onClick.AddListener(ChangeTurnState);
        manaBar.Init(this);
        description = GetDescription();
        UpdateDisplays();
    }

    private string GetDescription()
    {
        return $"<b><color=#{hexColour}>{GetCharacterName()} </b></color>\n Gain potential via matching the {manaType} spirit tokens. " +
            $"\n<b>Passives:</b>" +
            $"\nPassive 1 ( > 0 mana): {GetPassiveOneDesc()}" +
            $"\nPassive 2 ( >= {passiveValues.secPassiveThreshold} mana): {GetPassiveTwoDesc()}" +
            $"\nPassive 3 ( >= {passiveValues.thirdPassiveThreshold} mana): {GetPassiveThreeDesc()}";
    }

    private void StatusEffectsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (display != null)
        {
            display.DisplayStatus(statusEffects, sender, e);
        }
    }

    protected virtual void UpdateDisplays()
    {
        ChangeStateDisplay();
        DisplayPotential();
        healthText.text = $"{currentMana}";
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
                potentialText.color = Color.red;
                potentialText.text = potential.ToString();
                break;
            case TurnState.Retain:
                potentialText.color = Color.yellow;
                potentialText.text = potential.ToString();
                break;
            case TurnState.Heal:
                potentialText.color = Color.green;
                potentialText.text = potential.ToString();
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
        //play animation here...
        if(state == CharacterState.Alive)
        {
            int damage = GetDamage();
            if(damage > 0)
            {
                c.LoseMana(damage, this.GetType().Name);
                //delegate for apply on hit effects/extra damage
                if (onAttack != null)
                {
                    onAttack.Invoke(c, damage);
                }
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
    protected float GetHealingMult()
    {
        float healAmount = 1;
        foreach(DraggableItem item in items)
        {
            if(item.itemSO.healingMult > 0)
            {
                healAmount += item.itemSO.healingMult;
            }
        }
        return healAmount;
    }
    //character heal action (not ability related)
    public virtual void HealMana(int amount)
    {
        if (state == CharacterState.Dead)
            return;
        int healAmount = Mathf.RoundToInt(amount * GetHealingMult());
        currentMana += healAmount;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        } 
        if (state == CharacterState.Down && currentMana > 0)
            state = CharacterState.Alive;
        nextTurnPotential = 0;
        UpdateDisplays();
    }
    //method for taking damage on character, is given positive amount which is then removed from mana pool.
    public virtual void LoseMana(int amount, string source = "Unknown")
    {
        if(state != CharacterState.Dead && currentMana > 0)
        {
            amount -= GetDamageReduction();
            amount = Mathf.Max(amount,0);
            currentMana -= amount;
            if (onDamageTaken != null)
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
        if(statusEffects.Count > 0)
        {
            foreach(BaseStatus s in statusEffects)
            {
                if(s is IDurationStatus ds)
                {
                    ds.DecreaseDuration();
                }
            }
        }
        CheckStatusDurations();
    }
    //find an remove all status's with 0 or less duration/stacks since they're no longer effective
    protected void CheckStatusDurations()
    {
        List<BaseStatus> toRemove = new List<BaseStatus>();

        foreach (BaseStatus status in statusEffects)
        {
            if ((status is IDurationStatus d && d.Duration <= 0) ||
                (status is IStackableStatus s && s.StackCount <= 0) ||
                (status is IUseStatus u && u.UseCount <= 0))
            {
                toRemove.Add(status);
            }
        }

        foreach (BaseStatus status in toRemove)
        {
            statusEffects.Remove(status);
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
        for (int i = 0; i < statusEffects.Count; i++)
        {
            BaseStatus existing = statusEffects[i];
            if (status is Burn)
            {
                statusEffects.Add(status);
                return;
            }
            if (existing.GetType() == status.GetType())
            {
                if (existing is IStackableStatus existingStack && status is IStackableStatus newStack)
                {
                    existingStack.IncreaseStack(newStack.StackCount);
                }

                if (existing is IDurationStatus existingDur && status is IDurationStatus newDur)
                {
                    existingDur.IncreaseDuration(newDur.Duration);
                }

                if (existing is IUseStatus existingUse && status is IUseStatus newUse)
                {
                    existingUse.IncreaseUseCount(newUse.UseCount);
                }
                return;
            }
        }
        statusEffects.Add(status);
        if(status is IOnTurnEnd end)
        {
            onTurnEnd += end.OnTurnEnd;
        }
        if(status is IOnTurnStart start)
        {
            onTurnStart += start.OnTurnStart;
        }
        if(status is IOnDamageTakenItem dmg)
        {
            onDamageTaken += dmg.OnDamageTaken;
        }
        if(status is IOnAttackItem attack)
        {
            onAttack += attack.OnAttack;
        }
    }
    //remove a status effected from list on character
    public virtual void RemoveStatusEffect(int index)
    {
        BaseStatus status = statusEffects[index];
        if (status is IOnTurnEnd end)
        {
            onTurnEnd -= end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            onTurnStart -= start.OnTurnStart;
        }
        statusEffects.RemoveAt(index);
    }

    public void EquipItem(DraggableItem item)
    {
        items.Add(item);
        if (item is IOnAttackItem onAttackItem)
        {
            
            onAttack += onAttackItem.OnAttack;
        }
        if (item is IOnDamageTakenItem onDamageTakenItem)
        {
            onDamageTaken += onDamageTakenItem.OnDamageTaken;
        }
        if (item is IOnTurnEnd onTurnEndItem)
        {
            onTurnEnd += onTurnEndItem.OnTurnEnd;
        }
        if (item is IOnTurnStart onTurnStartItem)
        {
            onTurnStart += onTurnStartItem.OnTurnStart;
        }
        if (item is IStatItem statItem)
        {
            statItem.OnEquip();
        }
    }
    public void UnequipItem(DraggableItem item)
    {
        items.Remove(item);
        if (item is IOnAttackItem onAttackItem)
        { 
            onAttack -= onAttackItem.OnAttack;
        }
        if (item is IOnDamageTakenItem onDamageTakenItem)
        {
            onDamageTaken -= onDamageTakenItem.OnDamageTaken;
        }
        if (item is IOnTurnEnd onTurnEndItem)
        {
            onTurnEnd -= onTurnEndItem.OnTurnEnd;
        }
        if (item is IOnTurnStart onTurnStartItem)
        {
            onTurnStart -= onTurnStartItem.OnTurnStart;
        }
        if(item is IStatItem statItem)
        {
            statItem.OnUnequip();
        }
    }
    #region public facing getters
    public virtual int GetDamage_Public()
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
    public virtual int GetFlatDamage_Public()
    {
        int damage = 0;
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.flatDamageIncrease > 0)
            {
                damage += item.itemSO.flatDamageIncrease;
            }
        }
        return damage;
    }
    public virtual float GetDamageMult_Public()
    {
        float mult = 1f;
        foreach (DraggableItem item in items)
        {
            if (item.itemSO.damageMult > 0)
            {
                mult += item.itemSO.damageMult;
            }
        }
        return mult;
    }
    public virtual int GetDamageReduction_Public()
    {
        return GetDamageReduction();
    }
    public virtual int GetPotentialIncrease_Public()
    {
        return GetPotentialIncrease();
    }
    
    public virtual float GetPotentialMult_Public()
    {
        return GetPotentialMult();
    }
    public virtual float GetHealMult_Public()
    {
        return GetHealingMult();
    }
    public virtual float GetRetainMult_Public()
    {
        return GetRetainMult();
    }
    public virtual string GetCharacterName()
    {
        return $"{manaType} Character";
    }
    public abstract string GetPassiveOneDesc();
    public abstract string GetPassiveTwoDesc();
    public abstract string GetPassiveThreeDesc();

    #endregion
}
