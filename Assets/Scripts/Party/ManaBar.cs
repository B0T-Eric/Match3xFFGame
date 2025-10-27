using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image manaFillImage;
    public BaseCharacter character;
    public TextMeshProUGUI enemyHealth;
    public void Init(BaseCharacter c)
    {
        character = c;
        if(c is not EnemyCharacter)
        {
            if(ColorUtility.TryParseHtmlString("#"+c.hexColour, out Color colour))
            {
                manaFillImage.color = colour;
            }
        }
        UpdateHealthBar(c);
    }
    public void UpdateHealthBar(BaseCharacter c)
    {
        SetMana(c.currentMana);
        if (c is EnemyCharacter)
        {
            SetManaText(c.currentMana);
        }
    }
    private void SetManaText(int mana)
    {
        enemyHealth.text = $"{mana} / {character.statsSO.maxMana}";
    }
    private void SetMana(float mana)
    {
        manaFillImage.fillAmount = (float)(mana / character.statsSO.maxMana);
    }
}
