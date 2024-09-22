using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsObject", menuName = "ScriptableObjects/EnemyStatsObject")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    public float maxHealth = Random.Range(250f,500f);
}
