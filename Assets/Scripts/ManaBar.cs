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
    public Character character;
    // Start is called before the first frame update
    void Start()
    { 
        manaBar.maxValue = character.statsSO.maxMana;
        manaBar.value = character.currentMana;
        manaText.SetText($"{character.currentMana} / {character.statsSO.maxMana}");
        SetMana(character.currentMana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMana(float mana)
    {
        manaBar.value = mana;
    }
    public void SetManaText()
    {
        manaText.SetText($"{character.currentMana} / {character.statsSO.maxMana}");
    }
}
