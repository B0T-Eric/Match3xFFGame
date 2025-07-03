using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class EnemyCharacter : BaseCharacter
{
    //insure one enemy exists.
    private static EnemyCharacter instance;
    public static EnemyCharacter Instance {  get { return instance; } }
    //all targets retrieved from parties.
    public List<BaseCharacter> targets;
    //enemy stats
    [SerializeField]
    int maxDamage;
    [SerializeField]
    int minDamage;
    int currentTurnDamage;
    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        manaBar = GameObject.Find("EnemyHealthBar").GetComponent<ManaBar>();
        statusEffects = new System.Collections.ObjectModel.ObservableCollection<BaseStatus>();
        statusEffects.CollectionChanged += StatusEffectsChanged;
        GameContainer.Instance.DisplayEnemy(this);
        EnemyStatsSO statsSO = base.statsSO as EnemyStatsSO;
        statsSO.DetermineHealth();
        damageText = GameObject.Find("EnemyDmgText").GetComponent<TextMeshProUGUI>();
        maxMana = statsSO.maxMana;
        currentMana = maxMana;
        manaBar.Init(this);
        UpdateDisplays();
    }

    private void StatusEffectsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (display != null)
        {
            display.DisplayStatus(statusEffects);
        }
    }

    public void Die()
    {
        //disable game, remove enemy object, display rewards
        Destroy(gameObject);
    }

    public void SelectTargets() 
    {
        int targetCount = Random.Range(1, Party.Instance.members.Count);
        targets = GetTargets(targetCount);
        Debug.Log($"{targets.Count} target count");
    }
    private List<BaseCharacter> GetTargets(int targetCount)
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
        c.LoseMana(currentTurnDamage);
    }
    //get damage from a range based on a min and max value, then distribute the damage evenly between targets
    //if the damage is to 1 target make sure it's not over 60.
    protected override int GetDamage()
    {
        minDamage += Mathf.RoundToInt(minDamage * MapController.Instance.EnemyDamageModifier);
        maxDamage += Mathf.RoundToInt(maxDamage * MapController.Instance.EnemyDamageModifier);
        int damage = Random.Range(minDamage,maxDamage);
        foreach(BaseStatus status in statusEffects)
        {
            if(status is Weakness)
            {
                Weakness weakness = (Weakness)status;
                damage = Mathf.RoundToInt(damage * weakness.OutgoingDamageModifier);
            }
        }
        int targetCount = targets.Count;
        damage = damage / targetCount;
        return damage;
    }
    public override void LoseMana(int amount)
    {
        foreach(BaseStatus status in statusEffects)
        {
            if(status is Brittle)
            {
                Brittle brittle = (Brittle)status;
                amount = Mathf.RoundToInt(amount * brittle.IncomingDamageMultiplier);
            }
        }
        currentMana -= amount;
        UpdateDisplays();
    }
    protected override void UpdateDisplays()
    {
        manaBar.UpdateHealthBar(this);
        if (Party.Instance.HasKnowledgeOfEnemy)
        {
            //display damage and targets
            damageText.gameObject.SetActive(true);
            damageText.text = $"{currentTurnDamage}";
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }
    }
}
