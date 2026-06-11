using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { Attack, Heal, Retain, None}

//on attack params: BaseCharacter character = the targetted character by the attack (either enemy or party member)
public delegate void OnAttackDelegate(BaseCharacter character, int damage);
//on attack params: EnemyCharacter enemy = the targetted character by the attack (either enemy or party member)
public delegate void OnAttackEnemyDelegate(EnemyCharacter enemy, int damage);
//on damage taken params: BaseCharacter baseCharacter = the character which has just recieved damage.
public delegate void OnDamageTakenDelegate(BaseCharacter baseCharacter, int damage);
//on damage taken params: BaseCharacter baseCharacter = the character which has just recieved damage.
public delegate void OnEnemyDamageTakenDelegate(EnemyCharacter enemy, int damage);
//trigger on end turn effects.
public delegate void OnTurnEndDelegate();
//triggers effects on turn start
public delegate void OnTurnStartDelegate();
//On Hit delegate is when they're attacked not whenever damage is taken. the parameter character is the character who is attacking.
public delegate void OnHitDelegate(EnemyCharacter enemy);
public delegate void OnHitEnemyDelegate(BaseCharacter character);
//On Get Damage delegate returns increases/decreases to damage.
public delegate int OnGetDamage();
//On Get Damage delegate returns increases/decreases to damage (using original damage to return multiplied value).
public delegate int OnGetDamageMult(int damage);
//On Get Damage Reduction returns increases or decreases to damage reduction;
public delegate int OnGetDamageReduction();
//On Get Healing returns increases or decreases to mana gain;
public delegate int OnGetHealFlat();
//On Get Heal Mult returns multipliers to healing.
public delegate float OnGetHealMult();

[Serializable]
public abstract class BaseCharacter : MonoBehaviour
{
    #region Character Stat Values
    public PlayerCharacterStats stats;

    public CharacterStatsSO statsSO;
    public int potential;
    public int nextTurnPotential;
    public ManaType manaType;
    public ObservableCollection<BaseStatus> statusEffects;
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
    public OnAttackEnemyDelegate onAttack;
    public OnDamageTakenDelegate onDamageTaken;
    public OnHitDelegate onHit;
    public OnTurnEndDelegate onTurnEnd;
    public OnTurnStartDelegate onTurnStart;


    #endregion
    public BaseCompanion companion;
    public CharacterDescription characterDescription;
    public BaseAbility Ability;
    public virtual void Init()
    {
        statusEffects = new ObservableCollection<BaseStatus>();
        state = CharacterState.Alive;
        potential = 0;
        nextTurnPotential = 0;
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
        onTurnEnd += Ability.LowerCooldown;
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            if (b.name.Equals("AbilityButton"))
            {
                abilityButton = b;
            }
        }
        Ability.InitTextObj();
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
        healthText.text = $"{stats.CurrentMana}";
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
    public virtual void Attack(EnemyCharacter enemy)
    {
        if (state == CharacterState.Alive)
        {
            int damage = stats.GetDamage(potential);
            if (damage > 0)
            {
                attacks[selectedAttack].Attack(enemy,damage,this,GetCharacterName(),onAttack);
            }
        }
    }
    public virtual void HealMana(int amount)
    {
        if (state == CharacterState.Dead)//dead characters can't be healed
            return;
        int healAmount = stats.GetHealing(amount);

        stats.CurrentMana += healAmount;
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} was healed for {healAmount}");
        if(stats.CurrentMana > stats.MaxMana)
        {
            stats.CurrentMana = stats.MaxMana;
        } 
        //if they are down, heal them back to alive.
        if (state == CharacterState.Down && stats.CurrentMana > 0)
        {
            display.SetState(CharacterState.Alive);
        }
        UpdatePassives();
        UpdateDisplays();
    }
    //method for taking damage on character, is given positive amount which is then removed from mana pool.
    public virtual void LoseMana(int amount, string source = "Unknown")
    {
        if (state != CharacterState.Dead && stats.CurrentMana > 0)
        {
            display.DefendTrue();
            int baseResist = turnState == TurnState.Retain ? 5 : 0;
            amount -= stats.GetResist(baseResist);
            amount = Mathf.Max(amount, 0);
            IncreaseDamageTaken(amount);
            stats.CurrentMana -= amount;
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
        if (stats.CurrentMana <= 0)
        {
            if (state == CharacterState.Down)
            {
                stats.CurrentMana = 0;
                display.SetState(CharacterState.Dead);
            }
            //if the character has dropped below (or equal to) 0 and is alive set them to down.
            else if (state == CharacterState.Alive)
            {
                stats.CurrentMana = 1;
                display.SetState(CharacterState.Down);
            }
        }
    }

    public bool IsPassiveTwoActive => stats.CurrentMana >= stats.pTwo;
    public bool IsPassiveThreeActive => stats.CurrentMana >= stats.pThree;
    protected virtual void UpdatePassives()
    {

    }
    public virtual void GainPotential(int amount)
    {
        if (state == CharacterState.Alive)
        {
            potential += stats.GetPotential(amount);
        }
        UpdateDisplays();
    }
    
    public virtual void RetainPotential()
    {
        nextTurnPotential = stats.GetNextTurnPotential(potential);
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} retained {nextTurnPotential}");
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
        List<int> toRemove = StatusHandler.CheckStatusDurations(statusEffects);
        if(toRemove.Count > 0)
        {
            foreach (int index in toRemove)
            {
                StatusHandler.RemoveStatusEffect(statusEffects[index],this);
            }
        }
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
    #region Adding/Removing Items
    public void EquipItem(BaseItem item)
    {
        stats.Items.Add(item);
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
        if (item is IOnEnemyHitItem onHitItem)
        {
            onHit += onHitItem.OnHit;
        }
        if(item is IOnGetDamageReduction onDmgReduction)
        {
            stats.onGetResist += onDmgReduction.GetDamageReduction;
        }
        if (item is IOnGetDamage getDamage)
        {
            stats.onGetDmgFlat += getDamage.GetDamage;
        }
        if (item is IOnGetHealFlat onHeal)
        {
            stats.onGetHealFlat += onHeal.GetHealFlat;
        }
        if (item is IOnGetHealMult onHealMult)
        {
            stats.onGetHealMult += onHealMult.GetHealMult;
        }
        if (item is IOnGetDamageMult getDamageMult)
        {
            stats.onGetDmgMult += getDamageMult.GetDamageMult;
        }
    }
    public void UnequipItem(BaseItem item)
    {
        stats.Items.Remove(item);
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
        if(item is IOnEnemyHitItem onHitItem)
        {
            onHit -= onHitItem.OnHit;
        }
        if(item is IStatItem statItem)
        {
            statItem.OnUnequip();
        }
        if (item is IOnGetDamageReduction onDmgReduction)
        {
            stats.onGetResist -= onDmgReduction.GetDamageReduction;
        }
        if (item is IOnGetDamage getDamage)
        {
            stats.onGetDmgFlat -= getDamage.GetDamage;
        }
        if (item is IOnGetHealFlat onHeal)
        {
            stats.onGetHealFlat -= onHeal.GetHealFlat;
        }
        if (item is IOnGetHealMult onHealMult)
        {
            stats.onGetHealMult -= onHealMult.GetHealMult;
        }
        if (item is IOnGetDamageMult getDamageMult)
        {
            stats.onGetDmgMult -= getDamageMult.GetDamageMult;
        }
    }
    #endregion

    #region public facing getters
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
            $"\nPassive 2 ( >= {stats.pTwo} mana): {GetPassiveTwoDesc()}" +
            $"\nPassive 3 ( >= {stats.pThree} mana): {GetPassiveThreeDesc()}";
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
        stats.damageTaken += damage;
    }
    public void ResetDamageTaken()
    {
        stats.damageTaken = 0;
    }
    #endregion
}
