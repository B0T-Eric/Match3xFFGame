using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    private List<string> damageLog = new List<string>();
    int currentLogIndex = 0;
    protected string damageTakenString;
    TextMeshProUGUI damageTakenText, logTitleText, nameText;
    Button nextButton, lastButton, previousButton, firstButton;
    string enemyName;
    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        InitializeStats();
        InitializeDisplays();
        InitializeButtons();
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
        damageTakenText = GameObject.Find("DamageTaken").GetComponent<TextMeshProUGUI>();
        logTitleText = GameObject.Find("LogText").GetComponent<TextMeshProUGUI>();
        GameContainer.Instance.DisplayEnemy(this);
        manaBar.Init(this);
        nameText.text = enemyName;
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

    protected virtual void SelectTargets() 
    {
        int targetCount = Random.Range(1, Party.Instance.members.Count);
        targets = GetTargets(targetCount);
        Debug.Log($"{targets.Count} target count");
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
        damageTakenString = $"Damage Taken: ";
        logTitleText.text = $"Turn {currentLogIndex + 1}";
        CheckStatusDurations();
        UpdateButtons();
        SelectTargets();
        currentTurnDamage = GetDamage();
        UpdateDisplays();
    }
    public override void PreformTurn()
    {
        foreach (BaseCharacter target in targets)
        {
            Attack(target);
        }
    }
    public override void Attack(BaseCharacter c)
    {
        //animate
        c.LoseMana(currentTurnDamage, this.GetType().Name);
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
        damageTakenString += $" \n {amount} from {source} ";
        Debug.Log(damageTakenString);
        UpdateDisplays();
    }

    public void LogTurnDamage()
    {
        damageLog.Add(damageTakenString);
        DisplayDamageTaken();
    }
    public void DisplayDamageTaken()
    {
        damageTakenText.text = damageLog[currentLogIndex];
        logTitleText.text = $"Turn {currentLogIndex+1}";
    }
    protected void InitializeButtons()
    {
        firstButton = GameObject.Find("First").GetComponent<Button>();
        previousButton = GameObject.Find("Previous").GetComponent<Button>();
        lastButton = GameObject.Find("Last").GetComponent<Button>();
        nextButton = GameObject.Find("Next").GetComponent<Button>();
        firstButton.onClick.AddListener(FirstClick);
        previousButton.onClick.AddListener(PreviosClick);
        lastButton.onClick.AddListener(LastClick);
        nextButton.onClick.AddListener(NextClick);
    }
    public void UpdateButtons()
    {
        if (damageLog.Count == 0)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else if (currentLogIndex == 0 && damageLog.Count == 1)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else if (currentLogIndex == 0)
        {
            previousButton.interactable = false;
            firstButton.interactable = false;
            lastButton.interactable = true;
            nextButton.interactable = true;
        }
        else if (currentLogIndex == damageLog.Count - 1)
        {
            previousButton.interactable = true;
            firstButton.interactable = true;
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
            firstButton.interactable = true;
            lastButton.interactable = true;
            nextButton.interactable = true;
           }
   }
    public void LastClick()
    {
        currentLogIndex = damageLog.Count - 1;
        DisplayDamageTaken();
        UpdateButtons();
    }
    public void PreviosClick()
    {
        if(currentLogIndex > 0)
            currentLogIndex--;
        DisplayDamageTaken();
        UpdateButtons();
   }
    public void NextClick()
    {
        if(currentLogIndex < damageLog.Count - 1)
            currentLogIndex++;
        DisplayDamageTaken();
        UpdateButtons();
    }
    public void FirstClick()
    {
        currentLogIndex = 0;
        DisplayDamageTaken();
        UpdateButtons();
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
