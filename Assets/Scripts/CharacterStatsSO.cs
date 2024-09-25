using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharStatsObject", menuName = "ScriptableObjects/CharacterStatsObjects")]
public class CharacterStatsSO : ScriptableObject
{
    public int maxMana;
    public ManaType type;
    public int initialMana;
}
