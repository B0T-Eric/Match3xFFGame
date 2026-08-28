using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image manaFillImage;
    public BaseCharacter character;
    public TextMeshProUGUI manaText;
    public void Init(BaseCharacter c)
    {
        character = c;
        if(ColorUtility.TryParseHtmlString("#"+c.hexColour, out Color colour))
        {
            manaFillImage.color = colour;
        }
        //UpdateHealthBar(c);
    }
    public virtual void Init(EnemyCharacter e)
    {

    }
    public virtual void UpdateHealthBar(EnemyCharacter e)
    {
       
    }
    public void UpdateHealthBar(BaseCharacter c)
    {
        SetMana(c.stats.CurrentMana);
        SetManaText(c.stats.CurrentMana, manaText, character.stats.MaxMana);
    }
    protected void SetManaText(int mana, TextMeshProUGUI text, int maxMana)
    {
            text.text = $"{mana} / {maxMana}";
    }
    protected virtual void SetMana(float mana)
    {
        manaFillImage.fillAmount = (float)(mana / character.stats.MaxMana);
    }
}
