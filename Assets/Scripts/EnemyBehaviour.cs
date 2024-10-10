using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private static EnemyBehaviour _instance;
    public static EnemyBehaviour Instance { get { return _instance; } }

    public EnemyStatsScriptableObject enemyStats;

    public int currentHealth;
    private int damage;
    private List<Character> targets;
    public EnemyHealthBar healthBar;
    public bool isBurning = false;
    public int burnDamage = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        currentHealth = enemyStats.maxHealth;
        targets = new List<Character>();
    }
    public void SelectTargets()
    {
        //randomise target count - min 1, max all party members
        int targetCount = Random.Range(1, Party.Instance.members.Count);
        
        targets = GetTargets(targetCount);
        Debug.Log($"{targets.Count} target count");
    }
    List<Character> GetTargets(int targetCount)
    {
        List<Character> randTargets = new List<Character>();
        List<Character> availableTargets = new List<Character>(Party.Instance.members);
        for(int i = 0; i < targetCount; i++)
        {
            int randIndex = Random.Range(0, availableTargets.Count);
            randTargets.Add(availableTargets[randIndex]);
            availableTargets.RemoveAt(randIndex);
        }
        return randTargets;
    }
    public void Attack()
    {
        //randomly assign damage, then divide between targets
        damage = Random.Range(10, 30);
        SelectTargets();
        int damagePerTarget = Mathf.FloorToInt(damage / targets.Count);
        foreach (Character target in targets)
        {
            target.LoseMana(damagePerTarget);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //update health bar
        healthBar.SetHealth(currentHealth);
        healthBar.SetHealthText();
    }
    public void Die()
    {
        //disable game, remove enemy object, display rewards
        Destroy(gameObject);
    }

}
