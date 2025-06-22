using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharStatsObject", menuName = "ScriptableObjects/CharacterStatsObjects/Enemy")]
public class EnemyStatsSO : CharacterStatsSO
{
    public int Lower;
    public int Higher;
    public void DetermineHealth()
    {
        maxMana = (int)(Mathf.Round(UnityEngine.Random.Range(Lower, Higher) + 1) * MapController.Instance.EnemyHealthModifier);
    }
}