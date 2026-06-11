using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharStatsObject", menuName = "ScriptableObjects/CharacterStatsObjects")]
public class CharacterStatsSO : ScriptableObject
{
    public int maxMana; //60 - 5 = 55
    public int initialMana; //15 + 5 = 20
    public int passiveOne;
    public int passiveTwo;
    public int passiveThree;
}
