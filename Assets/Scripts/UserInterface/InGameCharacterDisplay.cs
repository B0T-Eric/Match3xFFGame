using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    internal void DisplayStatus(ObservableCollection<BaseStatus> statusEffects, object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (BaseStatus newStatus in e.NewItems)
            {
                statusManager.DisplayStatus(newStatus);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (BaseStatus oldStatus in e.OldItems)
            {
                statusManager.RemoveDisplayedStatus(oldStatus);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            statusManager.RemoveAllDisplays();
        }
    }
    public void DamageTakenVisual()
    {

    }
    public void ManaHealedVisual()
    {

    }
}
