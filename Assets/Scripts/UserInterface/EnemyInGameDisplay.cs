using System.Collections;
using UnityEngine;
public class EnemyInGameDisplay : InGameCharacterDisplay
{
    EnemyCharacter enemy;
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
    }
}