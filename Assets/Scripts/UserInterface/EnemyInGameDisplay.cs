using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyInGameDisplay : InGameCharacterDisplay
{
    EnemyCharacter enemy;
    [SerializeField]
    GameObject actionPrefab,pointerPrefab;
    [SerializeField]
    RectTransform intentContainer, pointerDisplay;
    public override void Attack()
    {
        enemy.Attack();
    }
    public override void DisplayCharacter(BaseCharacter c)
    {
        character = c;
        c.display = this;
        characterRender.sprite = character.sprite;
        animator = GetComponent<Animator>();
        enemy = c as EnemyCharacter;
        if (enemy is not WolfEnemy)
        {
            EnemyActionIndicator indicator = GetComponentInChildren<EnemyActionIndicator>();
            enemy.SetIndicator(indicator);
            foreach(EnemyAction action in enemy.GetEnemyActions())
            {
                GameObject obj = Instantiate(actionPrefab, intentContainer);
                Indicator i = obj.GetComponent<Indicator>();
                i.image.sprite = action.sprite;
                indicator.pointerPositions.Add(i.pointerSlot);
            }
            indicator.SetPointer(Instantiate(pointerPrefab, pointerDisplay).GetComponent<RectTransform>());
        }
    }
}