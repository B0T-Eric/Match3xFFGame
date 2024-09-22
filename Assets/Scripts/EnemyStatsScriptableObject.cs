using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsObject", menuName = "ScriptableObjects/EnemyStatsObject")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    public float maxHealth;
    private void OnEnable()
    {
        maxHealth = Random.Range(250, 501);
    }
}
