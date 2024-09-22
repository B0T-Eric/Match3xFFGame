using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharStatsObject", menuName = "ScriptableObjects/CharacterStatsObjects")]
public class CharacterStatsSO : ScriptableObject
{
    public float maxMana;
    public ManaType type;
    public float initialMana;
}
