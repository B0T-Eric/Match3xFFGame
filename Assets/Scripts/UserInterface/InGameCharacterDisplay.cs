using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class InGameCharacterDisplay : MonoBehaviour
{
    [SerializeField]
    BaseCharacter character;
    public bool IsDisplaying =false;
    [SerializeField]
    StatusDisplayManager statusManager;
    public void DisplayCharacter(BaseCharacter c)
    {
        character = c;
        c.display = this;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = character.sprite;
        IsDisplaying = true;
    }

    internal void DisplayStatus(ObservableCollection<BaseStatus> statusEffects)
    {
        statusManager.RemoveDisplayedStatus();
        foreach (BaseStatus status in statusEffects)
        {
            statusManager.DisplayStatus(status);
        }
    }
}
