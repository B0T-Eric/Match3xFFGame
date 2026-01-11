using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CompanionEventReward", menuName = "ScriptableObjects/Events/CompanionReward")]
public class CompanionEventReward : BaseEventReward
{
    [SerializeField]
    public GameObject companionPrefab;
    public void InitializeCompanion(BaseCharacter character, BaseEvent e)
    {
        GameObject companionObj = Instantiate(companionPrefab, character.GetCompanionLocation(),false);
        BaseCompanion companion = companionObj.GetComponent<BaseCompanion>();
        character.companion = companion;
        companion.Init(character);
        e.ToggleButtons();
    }
}
