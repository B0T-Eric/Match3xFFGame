using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public enum TurnState { Attack, Heal, Retain, None}

//on attack params: BaseCharacter character = the targetted character by the attack (either enemy or party member)
public delegate void OnAttackDelegate(BaseCharacter character, int damage);
//on damage taken params: BaseCharacter baseCharacter = the character which has just recieved damage.
public delegate void OnDamageTakenDelegate(BaseCharacter baseCharacter, int damage);
//trigger on end turn effects.
public delegate void OnTurnEndDelegate();
//triggers effects on turn start
public delegate void OnTurnStartDelegate();
//On Hit delegate is when they're attacked not whenever damage is taken. the parameter character is the character who is attacking.
public delegate void OnHitDelegate(BaseCharacter character);
//On Get Damage delegate returns increases/decreases to damage.
public delegate int OnGetDamage();
//On Get Damage delegate returns increases/decreases to damage (using original damage to return multiplied value).
public delegate int OnGetDamageMult(int damage);
//On Get Damage Reduction returns increases or decreases to damage reduction;
public delegate int OnGetDamageReduction();
//On Get Healing returns increases or decreases to mana gain;
public delegate int OnGetHealing();
//On Get mult
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
    public int damageTaken = 0;
    protected int attackType = 0;
    #endregion
    #region Character States
    public CharacterState state;
    public TurnState turnState;
    public bool isStunned = false;
    #endregion
    #region Display
    public string hexColour;
    public string description;
    public GameObject partyUi;
    public Sprite sprite, downSprite, deadSprite;
    public GameObject InGameDisplayPrefab;
    public InGameCharacterDisplay display;
    [SerializeField]
    private CharacterTurnPicker turnPicker;
    public ManaBar manaBar;
    public TextMeshProUGUI potentialText, healthText;
    public CharacterInventorySlot[] inventorySlots;
    public Button abilityButton;
    public Button turnStateButton;
    #endregion
    #region Character Inventory
    public List<DraggableItem> items;
    public OnAttackDelegate onAttack;
    public OnDamageTakenDelegate onDamageTaken;
    public OnHitDelegate onHit;
    public OnTurnEndDelegate onTurnEnd;
    public OnTurnStartDelegate onTurnStart;
    public OnGetDamage onGetDamage;
    public OnGetDamageReduction onGetDamageReduction;
    public OnGetHealing onGetHealing;
    public OnGetDamageMult onGetDamageMult;
    #endregion
    public BaseCompanion companion;
    public CharacterDescription characterDescription;
    public BaseAbility Ability;
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
        inventorySlots = GetComponentsInChildren<CharacterInventorySlot>(true);
        turnPicker = GetComponentInChildren<CharacterTurnPicker>();
        turnPicker.SetCharacter(this);

        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            if (b.name.Equals("AbilityButton"))
            {
                abilityButton = b;
            }
        }
        abilityButton.onClick.AddListener(AbilityButtonClick);
        manaBar.Init(this);
        description = GetDescription();
        UpdateDisplays();
    }
    protected virtual void AbilityButtonClick()
    {
        //activate characters unique ability
    }

    private void StatusEffectsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (display != null)
        {
            display.DisplayStatus(statusEffects, sender, e);
        }
    }
    public RectTransform GetCompanionLocation()
    {
        foreach (RectTransform rt in GetComponentsInChildren<RectTransform>(true))
        {
            if (rt.name == "CompanionPosition")
                return rt;
        }

        Debug.LogError("CompanionPosition not found!");
        return null;
    }
    protected virtual void UpdateDisplays()
    {
        DisplayPotential();
        healthText.text = $"{currentMana}";
        manaBar.UpdateHealthBar(this);
    }
    public void UpdateDisplays_Public()
    {
        UpdateDisplays();
    }
    public virtual void CombatStart()
    {

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
    public virtual async Task<bool> PreformTurn()
    {
        //call the ingame character methods that call the animations to happen, the animations then have the events in them to do "Attack" or "Heal" or "Retain".
        if (!isStunned)
        {
            switch (turnState)
            {
                case TurnState.Attack: display.AttackTrue(); break;
                case TurnState.Heal: display.HealTrue(); break;
                case TurnState.Retain: display.RetainTrue(); break;
            }
            if (companion != null)
            {
                companion.PreformAction();
            }
            UpdateDisplays();
            await Task.Delay(1000); 
        }
        return true;
    }
    public List<BasicAttack> attacks = new List<BasicAttack>();
    public int selectedAttack = 0;
    public virtual void Attack(BaseCharacter c)
    {
        if (state == CharacterState.Alive)
        {
            int damage = GetDamage();
            if (damage > 0)
            {
                attacks[selectedAttack].Attack(c,damage,this,GetCharacterName(),onAttack);
            }
        }
    }

    protected virtual int GetDamage()
    {
        int damage = potential;
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
        if(onGetDamage != null)
        {
            foreach (OnGetDamage getDamage in onGetDamage.GetInvocationList().Cast<OnGetDamage>())
            {
                damage += getDamage.Invoke();
            }
        }
        int baseDamage = damage;
        int bonusDamage = 0;
        if(onGetDamageMult != null)
        {
            foreach(OnGetDamageMult getDamageMult in onGetDamageMult.GetInvocationList().Cast<OnGetDamageMult>())
            {
                bonusDamage += getDamageMult.Invoke(baseDamage);
            }
        }
        damage = bonusDamage + baseDamage;
        return damage;
    }
    protected int GetDamageReduction()
    {
        int dmgReduction = 0;
        if(turnState == TurnState.Retain)
        {
            dmgReduction += 5;
        }
        if(onGetDamageReduction != null)
        {
            foreach (OnGetDamageReduction reduction in onGetDamageReduction.GetInvocationList().Cast<OnGetDamageReduction>())
            {
                dmgReduction += reduction.Invoke();
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
        if (onGetHealing != null)
        {
            foreach (OnGetHealing healing in onGetHealing.GetInvocationList().Cast<OnGetHealing>())
            {
                healAmount += healing.Invoke();
            }
        }
        return healAmount;
    }
    public virtual void HealMana(int amount)
    {
        if (state == CharacterState.Dead)//dead characters can't be healed
            return;
        int healAmount = Mathf.RoundToInt(amount * GetHealingMult());

        currentMana += healAmount;
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} was healed for {healAmount}");
        if(currentMana > maxMana )
        {
            currentMana = maxMana;
        } 
        //if they are down, heal them back to alive.
        if (state == CharacterState.Down && currentMana > 0)
        {
            display.SetState(CharacterState.Alive);
        }
        UpdatePassives();
        UpdateDisplays();
    }
    //method for taking damage on character, is given positive amount which is then removed from mana pool.
    public virtual void LoseMana(int amount, string source = "Unknown")
    {
        if (state != CharacterState.Dead && currentMana > 0)
        {
            display.DefendTrue();
            amount -= GetDamageReduction();
            amount = Mathf.Max(amount, 0);
            IncreaseDamageTaken(amount);
            currentMana -= amount;
            if (onDamageTaken != null)
            {
                foreach (OnDamageTakenDelegate onDamageTaken in onDamageTaken.GetInvocationList().Cast<OnDamageTakenDelegate>())
                {
                    onDamageTaken.Invoke(this, amount);
                }
            }
            CheckState();
            UpdatePassives();
            CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} took {amount} damage from {source}");
        }
        UpdateDisplays();
    }

    public void CheckState()
    {
        //if the character is down and has dropped below (or equal to) 0, set them to dead.
        if (currentMana <= 0)
        {
            if (state == CharacterState.Down)
            {
                currentMana = 0;
                display.SetState(CharacterState.Dead);
            }
            //if the character has dropped below (or equal to) 0 and is alive set them to down.
            else if (state == CharacterState.Alive)
            {
                currentMana = 1;
                display.SetState(CharacterState.Down);
            }
        }
    }

    public bool IsPassiveTwoActive => currentMana >= passiveValues.secPassiveThreshold;
    public bool IsPassiveThreeActive => currentMana >= passiveValues.thirdPassiveThreshold;
    protected virtual void UpdatePassives()
    {

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
        if (state == CharacterState.Alive)
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
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} retained {rounded}");
    }
    //handle start of turn actions
    public virtual void TurnStart()
    {
        if (isStunned)
        {
            isStunned = false;
        }
        if (state != CharacterState.Dead)
        {
            potential = nextTurnPotential;
            
        }
        if (onTurnStart != null)
        {
            foreach (OnTurnStartDelegate onTurnStart in onTurnStart.GetInvocationList())
            {
                onTurnStart.Invoke();
            }
        }
        nextTurnPotential = 0;
        UpdateDisplays();
        UpdatePassives();
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
        List<int> toRemove = new List<int>();

        foreach (BaseStatus status in statusEffects)
        {
            if ((status is IDurationStatus d && d.Duration <= 0) ||
                (status is IStackableStatus s && s.StackCount <= 0) ||
                (status is IUseStatus u && u.UseCount <= 0))
            {
                toRemove.Add(statusEffects.IndexOf(status));
            }
        }

        foreach (int index in toRemove)
        {
            RemoveStatusEffect(statusEffects[index]);
        }
    }
    //calculate retain amount after items / other effects
    protected virtual float GetRetainMult()
    {
        return 0.75f;
    }
    //this should change the current turns state (action) when the player chooses it.
    public void SetTurnState(TurnState state)
    {
        turnState = state;
        UpdateDisplays();
    }
    public void ResetTurnState() 
    {
        turnState = TurnState.Retain;
        UpdateDisplays();
    }
    #region Adding/Removing Statuses and Items
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
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            onGetDamageReduction += onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            onGetDamage += getDamage.GetDamage;
        }
        if (status is IOnGetHealing onHeal)
        {
            onGetHealing += onHeal.GetHealing;
        }
        if (status is IOnApply)
        {
            status.PreformEffect();
        }
        if(status is IOnGetDamageMult getDamageMult)
        {
            onGetDamageMult += getDamageMult.GetDamageMult;
        }
    }
    //remove a status effected from list on character
    public virtual void RemoveStatusEffect(BaseStatus status)
    {
        if (status is IOnTurnEnd end)
        {
            onTurnEnd -= end.OnTurnEnd;
        }
        if (status is IOnTurnStart start)
        {
            onTurnStart -= start.OnTurnStart;
        }
        if (status is IOnAttackItem attack)
        {
            onAttack += attack.OnAttack;
        }
        if (status is IOnGetDamageReduction onDmgReduction)
        {
            onGetDamageReduction -= onDmgReduction.GetDamageReduction;
        }
        if (status is IOnGetDamage getDamage)
        {
            onGetDamage -= getDamage.GetDamage;
        }
        if (status is IOnGetHealing onHeal)
        {
            onGetHealing -= onHeal.GetHealing;
        }
        if (status is IOnGetDamageMult getDamageMult)
        {
            onGetDamageMult -= getDamageMult.GetDamageMult;
        }
        statusEffects.Remove(status);
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
        if (item is IOnHitItem onHitItem)
        {
            onHit += onHitItem.OnHit;
        }
        if(item is IOnGetDamageReduction onDmgReduction)
        {
            onGetDamageReduction += onDmgReduction.GetDamageReduction;
        }
        if (item is IOnGetDamage getDamage)
        {
            onGetDamage += getDamage.GetDamage;
        }
        if (item is IOnGetHealing onHeal)
        {
            onGetHealing += onHeal.GetHealing;
        }
        if (item is IOnGetDamageMult getDamageMult)
        {
            onGetDamageMult += getDamageMult.GetDamageMult;
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
        if(item is IOnHitItem onHitItem)
        {
            onHit -= onHitItem.OnHit;
        }
        if(item is IStatItem statItem)
        {
            statItem.OnUnequip();
        }
        if (item is IOnGetDamageReduction onDmgReduction)
        {
            onGetDamageReduction -= onDmgReduction.GetDamageReduction;
        }
        if (item is IOnGetDamage getDamage)
        {
            onGetDamage -= getDamage.GetDamage;
        }
        if (item is IOnGetHealing onHeal)
        {
            onGetHealing -= onHeal.GetHealing;
        }
        if (item is IOnGetDamageMult getDamageMult)
        {
            onGetDamageMult -= getDamageMult.GetDamageMult;
        }
    }
    #endregion

    #region public facing getters
    public virtual int GetDamage_Public()
    {
        int damage = potential;
        if(onGetDamage != null)
        {
            foreach (OnGetDamage onGet in onGetDamage.GetInvocationList().Cast<OnGetDamage>())
            {
                damage += onGet.Invoke();
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
    public virtual string GetCharacterManaType()
    {
        return $"{manaType}";
    }
    private string GetDescription()
    {
        return $"<b><color=#{hexColour}>{GetCharacterName()} </b></color>\n {GetMatchDesc()}  " +
            $"\n<b>Passives:</b>" +
            $"\nPassive 1 ( > 0 mana): {GetPassiveOneDesc()}" +
            $"\nPassive 2 ( >= {passiveValues.secPassiveThreshold} mana): {GetPassiveTwoDesc()}" +
            $"\nPassive 3 ( >= {passiveValues.thirdPassiveThreshold} mana): {GetPassiveThreeDesc()}";
    }
    public virtual string GetMatchDesc()
    {
        return $"Gain potential via matching the {manaType} spirit tokens.";
    }
    public abstract string GetPassiveOneDesc();
    public abstract string GetPassiveTwoDesc();
    public abstract string GetPassiveThreeDesc();

    #endregion
    #region Damage Taken
    public void IncreaseDamageTaken(int damage)
    {
        damageTaken += damage;
    }
    public void ResetDamageTaken()
    {
        damageTaken = 0;
    }
    #endregion
}
