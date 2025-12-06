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
    [SerializeField]
    SpriteRenderer companionRender, characterRender;
    public void DisplayCharacter(BaseCharacter c)
    {
        character = c;
        c.display = this;
        characterRender = GetComponent<SpriteRenderer>();
        characterRender.sprite = character.sprite;
        companionRender = transform.GetComponentInChildren<SpriteRenderer>();
        if(character.companion != null)
        {
            companionRender.sprite = character.companion.GetSprite();
        }
        else
        {
            companionRender.sprite = null;
        }
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
