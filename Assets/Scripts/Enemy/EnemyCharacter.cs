using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnemyCharacter : MonoBehaviour
{
    //insure one enemy exists.
    private static EnemyCharacter instance;
    public static EnemyCharacter Instance {  get { return instance; } }
    //all targets retrieved from parties.
    public List<BaseCharacter> targets;
    //enemy stats
    [SerializeField]
    protected int maxDamage;
    [SerializeField]
    protected int minDamage;

    public Sprite sprite;
    TextMeshProUGUI nameText;
    public EnemyManaBar manaBar;
    public ObservableCollection<BaseStatus> statusEffects;
    public TextMeshProUGUI damageText;
    [SerializeField]
    public List<EnemyAction> enemyActions;
    protected int actionIndex;
    protected EnemyActionIndicator Indicator;
    string enemyName;
    public EnemyInGameDisplay display;
    [SerializeField]
    EnemyStatsSO statsSO;
    public EnemyCharacterStats stats;
    public bool isStunned = false;
    public OnAttackDelegate onAttack;
    public OnEnemyDamageTakenDelegate onDamageTaken;
    public OnHitEnemyDelegate onHit;
    public OnTurnEndDelegate onTurnEnd;
    public OnTurnStartDelegate onTurnStart;
    private void Awake()
    {
        instance = this;
    }
    public List<EnemyAction> GetEnemyActions()
    {
        return enemyActions;
    }
    public void SetIndicator(EnemyActionIndicator indicator)
    {
        Indicator = indicator;
    }
    public virtual void Init()
    {
        InitializeStats();
        InitializeDisplays();

        UpdateDisplays();
    }
    protected virtual void InitializeDisplays()
    {
        nameText = GameObject.Find("EnemyNameText").GetComponent<TextMeshProUGUI>();
        manaBar = GameObject.Find("EnemyHealthBar").GetComponent<EnemyManaBar>();
        statusEffects = new ObservableCollection<BaseStatus>();
        statusEffects.CollectionChanged += StatusEffectsChanged;
        damageText = GameObject.Find("EnemyDmgText").GetComponent<TextMeshProUGUI>();
        GameContainer.Instance.DisplayEnemy(this);
        manaBar.InitEnemy(this);
        nameText.text = GetCharacterName();
    }
    public virtual string GetCharacterName()
    {
        return $"{enemyName}";
    }
    protected virtual void InitializeStats()
    {
        stats = new EnemyCharacterStats();
        statsSO.DetermineHealth();
        stats.MaxMana = statsSO.maxMana;
        stats.CurrentMana = statsSO.maxMana;
        minDamage = Mathf.RoundToInt(minDamage * MapController.Instance.EnemyDamageModifier);
        maxDamage = Mathf.RoundToInt(maxDamage * MapController.Instance.EnemyDamageModifier);
        enemyName = statsSO.EnemyName;
    }   
    public virtual int GetDamage_Public()
    {
        return stats.Damage;
    }
    private void StatusEffectsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (display != null)
        {
            display.DisplayStatus(statusEffects, sender, e);
        }
    }
    public void Die()
    {
        //disable game, remove enemy object, display rewards
        Destroy(gameObject);
    }
    protected virtual void SelectTargets() 
    {
        List<BaseCharacter> validTargets = new List<BaseCharacter>();
        foreach (BaseCharacter bc in Party.Instance.members)
        {
            if (bc.state == CharacterState.Alive || bc.state == CharacterState.Down)
            {
                validTargets.Add(bc);
            }
        }
        targets = validTargets;
    }
    protected virtual List<BaseCharacter> GetTargets(int targetCount)
    {
        List<BaseCharacter> randTargets = new List<BaseCharacter>();
        List<BaseCharacter> availableTargets = new List<BaseCharacter>(Party.Instance.members);
        for (int i = 0; i < targetCount; i++)
        {
            int randIndex = Random.Range(0, availableTargets.Count);
            randTargets.Add(availableTargets[randIndex]);
            availableTargets.RemoveAt(randIndex);
        }
        return randTargets;
    }
    public virtual void TurnStart()
    {

        if (onTurnStart != null)
        {
            foreach (OnTurnStartDelegate onTurnStart in onTurnStart.GetInvocationList().Cast<OnTurnStartDelegate>())
            {
                onTurnStart.Invoke();
            }
        }
        if (isStunned)
        {
            isStunned = false;
        }
        List<int> toRemove = StatusHandler.CheckStatusDurations(statusEffects);
        if (toRemove.Count > 0)
        {
            foreach (int index in toRemove)
            {
                StatusHandler.RemoveStatusEffect(statusEffects[index], this);
            }
        }
        SelectTargets();
        stats.Damage = stats.GetDamage();
        UpdateDisplays();
    }
    public void TurnEnd()
    {
        if (onTurnEnd != null)
        {
            foreach (OnTurnEndDelegate onTurnEnd in onTurnEnd.GetInvocationList())
            {
                onTurnEnd.Invoke();
            }
        }
        if (statusEffects.Count > 0)
        {
            foreach (BaseStatus s in statusEffects)
            {
                if (s is IDurationStatus ds)
                {
                    ds.DecreaseDuration();
                }
            }
        }
        List<int> toRemove = StatusHandler.CheckStatusDurations(statusEffects);
        if (toRemove.Count > 0)
        {
            foreach (int index in toRemove)
            {
                StatusHandler.RemoveStatusEffect(statusEffects[index], this);
            }
        }
    }
    public virtual async Task<bool> PreformTurn()
    {
        if (!isStunned)
        {
            if (actionIndex == enemyActions.Count)
            {
                actionIndex = 0;
            }
            else
            {
                EnemyAction action = enemyActions[actionIndex];
                HandleActions(action, actionIndex);
                actionIndex++;
                Indicator.ShiftPointer();
            }
            await Task.Delay(1000); 
        }
        return true;
    }
    protected virtual void HandleActions(EnemyAction action, int index)
    {
        if (action is EnemyAttack)
        {
            action.PreformAction(this);
        }
        else if (action is EnemyHeal)
        {
            action.PreformAction(this);
        }
    }
    public virtual void Attack()
    {
        foreach (BaseCharacter target in targets)
        {
            Attack(target);
        }
    }
    public virtual void Attack(BaseCharacter c)
    {
        c.LoseMana(stats.Damage, GetCharacterName());
    }
    public virtual void LoseMana(int amount, string source = "Unknown")
    {
        display.DefendTrue();
        if (statusEffects.Count > 0)
        {
            foreach (BaseStatus status in statusEffects)
            {
                if (status is Brittle)
                {
                    Brittle brittle = (Brittle)status;
                    amount = Mathf.RoundToInt(amount * brittle.ApplyBrittle());
                }
            }
        }
        stats.CurrentMana -= amount;
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} took {amount} damage from {source}");
        UpdateDisplays();
    }
    public void IncreaseDamageTaken(int damage)
    {
        stats.damageTaken += damage;
    }
    public void ResetDamageTaken()
    {
        stats.damageTaken = 0;
    }
    protected virtual void UpdateDisplays()
    {
        manaBar.UpdateHealthBar(this);
        if (Party.Instance.HasKnowledgeOfEnemy)
        {
            //display damage and targets
            damageText.gameObject.SetActive(true);
            damageText.text = $"{stats.Damage}";
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }
    }
}
