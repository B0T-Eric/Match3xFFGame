using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsObject", menuName = "ScriptableObjects/EnemyStatsObject")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    public int minRoll;
    public int maxRoll;
    public int maxHealth;
    private void OnEnable()
    {
        maxHealth = Random.Range(minRoll, maxRoll);
    }
}
