using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && EnemyCharacter.Instance.stats.CurrentMana > 0)
        {
            EnemyCharacter.Instance.LoseMana(EnemyCharacter.Instance.stats.MaxMana);
        }
    }
}
