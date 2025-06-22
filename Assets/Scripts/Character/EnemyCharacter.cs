using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
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
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }
    public override void Init()
    {
        EnemyStatsSO statsSO = base.statsSO as EnemyStatsSO;
        statsSO.DetermineHealth();
        maxMana = statsSO.maxMana;
        currentMana = maxMana;
        manaBar.Init(this);
        UpdateDisplays();
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
        int targetCount = targets.Count;
        return (damage / targetCount);
    }
    public override void LoseMana(int amount)
    {
        currentMana -= amount;
        UpdateDisplays();
    }
    protected override void UpdateDisplays()
    {
        manaBar.SetMana(currentMana);
        manaBar.SetManaText();
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
