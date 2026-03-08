using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [SerializeField]
    protected int abilityCooldown;
    protected int cooldown;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected BaseCharacter character;
    public virtual void ActivateAbility()
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
        character.UpdateDisplays_Public();
    }
    private void SpendMana()
    {
        character.currentMana -= cost;
        character.CheckState();
    }
    private void SetCooldown()
    {
        cooldown = abilityCooldown;
    }
    public virtual void ActivateAbility(BaseCharacter target)
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
    }
    public virtual void ActivateAbility(int damage)
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
    }
    public void LowerCooldown()
    {
        if(cooldown > 0)
        {
            cooldown--;
        }
        if (cooldown == 0)
        {
            ChangeButtonEnable(true);
        }        
    }
    public void ChangeButtonEnable(bool value)
    {
        character.abilityButton.enabled = value;
        character.abilityButton.interactable = value;
    }
}
