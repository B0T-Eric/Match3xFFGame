using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    //mana is the resource required for using abilities, and determines characters who are alive or dead
    public float currentMana;
    CharacterState state;
    public CharacterStatsSO statsSO;
    [SerializeField]
    public GameObject prefab;
    public ManaBar manaBar;
    private void Start()
    {
        currentMana = statsSO.initialMana;
    }
    public void LoseMana(float amount)
    {
        if(state != CharacterState.Dead)
        {
            if (state == CharacterState.Alive)
            {
                if (currentMana - amount <= 0 || currentMana - amount == 1)
                {
                    state = CharacterState.Down;
                    currentMana = 1;
                }
                else
                {
                    currentMana -= amount;
                }
            }
            else if (state == CharacterState.Down)
            {
                if (currentMana - amount < 0)
                {
                    state = CharacterState.Dead;
                }
            }
        }      
    }
    public void GainMana(float amount)
    {
        if (state != CharacterState.Dead)
        {
            if (state != CharacterState.Down)
            {
                if (currentMana + amount > statsSO.maxMana)
                {
                    currentMana = statsSO.maxMana;
                }
                else
                {
                    currentMana += amount;
                }
            }
            else
            {
                currentMana += amount;
                state = CharacterState.Alive;
            }
        }
    }
    private void FixedUpdate()
    {
        manaBar.SetMana(currentMana);
        manaBar.SetManaText();
    }
}
