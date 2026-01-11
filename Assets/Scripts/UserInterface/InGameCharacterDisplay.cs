using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

public class InGameCharacterDisplay : MonoBehaviour
{
    protected BaseCharacter character;
    public bool IsDisplaying =false;
    [SerializeField]
    protected StatusDisplayManager statusManager;
    [SerializeField]
    protected SpriteRenderer characterRender;
    protected Animator animator;
    SpriteRenderer companionRender;
    public virtual void DisplayCharacter(BaseCharacter c)
    {
        character = c;
        c.display = this;
        characterRender.sprite = character.sprite;
        animator = GetComponent<Animator>();
        if(companionRender != null)
        {
            if(character.companion != null)
            {
                companionRender.sprite = character.companion.GetSprite();
            }
            else
            {
                companionRender.sprite = null;
                Color color = companionRender.color;
                color.a = 0f;
                companionRender.color = color;
            }
        }
            IsDisplaying = true;
    }
    //call the Attack(BaseCharacter c) method
    public virtual void Attack()
    {
        character.Attack(EnemyCharacter.Instance);
    }
    //call the character.Heal() method
    public void Heal()
    {
        character.HealMana(character.potential);
    }
    //call the character.RetainPotential method
    public void Retain()
    {
        character.RetainPotential();
    }
    public void AttackTrue()
    {
        animator.SetBool("Attacking",true);
    }
    public void AttackFalse()
    {
        animator.SetBool("Attacking",false);
    }
    public void DefendTrue()
    {
        animator.SetBool("TakingDamage",true);
    }
    public void DefendFalse()
    {
        animator.SetBool("TakingDamage",false);
    }
    public void HealTrue()
    {
        animator.SetBool("Healing",true);
    }
    public void HealFalse()
    {
        animator.SetBool("Healing",false);
    }
    public void RetainTrue()
    {
        animator.SetBool("Retaining",true);
    }
    public void RetainFalse()
    {
        animator.SetBool("Retaining", false);
    }
    public virtual void DisplayStatus(ObservableCollection<BaseStatus> statusEffects, object sender, NotifyCollectionChangedEventArgs e)
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
