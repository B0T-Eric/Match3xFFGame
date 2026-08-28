using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManaBar : ManaBar
{
    public EnemyCharacter enemy;
    
    public void InitEnemy(EnemyCharacter e)
    {
        enemy = e;
        manaText.text = "init";
        UpdateHealthBar(enemy);
    }
    public override void UpdateHealthBar(EnemyCharacter e)
     {
        SetMana(e.stats.CurrentMana);
        SetManaText(e.stats.CurrentMana, manaText, e.stats.MaxMana);
    }
    protected override void SetMana(float mana)
    {
        manaFillImage.fillAmount = (float)(mana / enemy.stats.MaxMana);
    }
}
