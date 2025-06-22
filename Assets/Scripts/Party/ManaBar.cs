using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaBar;
    public TextMeshProUGUI manaText;
    public BaseCharacter character;
    
    public void Init(BaseCharacter c)
    {
        character = c;
        manaBar.maxValue = character.statsSO.maxMana;
        manaBar.value = character.currentMana;
        manaText.SetText($"{character.currentMana} / {character.statsSO.maxMana}");
    }
    public void SetMana(float mana)
    {
        manaBar.value = mana;
    }
    public void SetManaText()
    {
        if(character != null)
        manaText.SetText($"{character.currentMana} / {character.statsSO.maxMana}");
    }
}
