using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : BaseCharacter
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
    protected int currentTurnDamage;
    TextMeshProUGUI nameText;

    [SerializeField]
    public List<EnemyAction> enemyActions;
    protected int actionIndex;
    protected EnemyActionIndicator Indicator;
    string enemyName;
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
    public override void Init()
    {
        InitializeStats();
        InitializeDisplays();

        UpdateDisplays();
    }
    #region descriptions
    public override string GetPassiveOneDesc()
    {
        return $"";
    }
    public override string GetPassiveTwoDesc()
    {
        return $"";
    }
    public override string GetPassiveThreeDesc()
    {
        return $"";
    }
    #endregion
    protected virtual void InitializeDisplays()
    {
        nameText = GameObject.Find("EnemyNameText").GetComponent<TextMeshProUGUI>();
        manaBar = GameObject.Find("EnemyHealthBar").GetComponent<ManaBar>();
        statusEffects = new System.Collections.ObjectModel.ObservableCollection<BaseStatus>();
        statusEffects.CollectionChanged += StatusEffectsChanged;
        potentialText = GameObject.Find("EnemyDmgText").GetComponent<TextMeshProUGUI>();
        GameContainer.Instance.DisplayEnemy(this);
        manaBar.Init(this);
        nameText.text = GetCharacterName();
    }
    public override string GetCharacterName()
    {
        return $"{enemyName}";
    }
    protected void InitializeStats()
    {
        EnemyStatsSO statsSO = base.statsSO as EnemyStatsSO;
        statsSO.DetermineHealth();
        maxMana = statsSO.maxMana;
        currentMana = maxMana;
        minDamage = Mathf.RoundToInt(minDamage * MapController.Instance.EnemyDamageModifier);
        maxDamage = Mathf.RoundToInt(maxDamage * MapController.Instance.EnemyDamageModifier);
        enemyName = statsSO.EnemyName;
    }
    public override int GetDamage_Public()
    {
        return currentTurnDamage;
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

    //protected virtual void SelectTargets() 
    //{
    //    int targetCount = Random.Range(1, Party.Instance.members.Count);
    //    targets = GetTargets(targetCount);
    //}
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
    public override void TurnStart()
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
        CheckStatusDurations();
        SelectTargets();
        currentTurnDamage = GetDamage();
        UpdateDisplays();
    }
    public override async Task<bool> PreformTurn()
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
    public void Attack()
    {
        foreach (BaseCharacter target in targets)
        {
            Attack(target);
        }
    }
    public override void Attack(BaseCharacter c)
    {
        c.LoseMana(currentTurnDamage, GetCharacterName());
    }
    //get damage from a range based on a min and max value, then distribute the damage evenly between targets
    //if the damage is to 1 target make sure it's not over 60.
    protected override int GetDamage()
    {
        int damage = Random.Range(minDamage,maxDamage);
        if(statusEffects.Count > 0)
        {
            foreach (BaseStatus status in statusEffects)
            {
                if (status is Weakness)
                {
                    Weakness weakness = (Weakness)status;
                    damage = Mathf.RoundToInt(damage * weakness.ApplyWeakness());
                }
            }

        }
        
        int targetCount = targets.Count;
        damage = damage / targetCount;
        return damage;
    }
    public override void LoseMana(int amount, string source = "Unknown")
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
        currentMana -= amount;
        CombatLog.Instance.AmendTextToLog($"{GetCharacterName()} took {amount} damage from {source}");
        UpdateDisplays();
    }


    protected override void UpdateDisplays()
    {
        manaBar.UpdateHealthBar(this);
        if (Party.Instance.HasKnowledgeOfEnemy)
        {
            //display damage and targets
            potentialText.gameObject.SetActive(true);
            potentialText.text = $"{currentTurnDamage}";
        }
        else
        {
            potentialText.gameObject.SetActive(false);
        }
    }
}
