using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    EnemyBehaviour enemyBehaviour;
    void Start()
    {
        enemyBehaviour = EnemyBehaviour.Instance;
        healthBar.maxValue = enemyBehaviour.enemyStats.maxHealth;
        healthBar.value = enemyBehaviour.currentHealth;
        healthText.SetText($"Health: {enemyBehaviour.currentHealth} / {enemyBehaviour.enemyStats.maxHealth}");
        SetHealth(enemyBehaviour.currentHealth);
    }
    public void SetHealth(float health)
    {
        healthBar.value = health;
    }
    public void SetHealthText()
    {
        healthText.SetText($"Health: {enemyBehaviour.currentHealth} / {enemyBehaviour.enemyStats.maxHealth}");
    }
}
