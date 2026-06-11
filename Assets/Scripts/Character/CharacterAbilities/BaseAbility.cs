using TMPro;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cooldownText, costText;
    [SerializeField]
    protected int abilityCooldown;
    protected int cooldown;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected BaseCharacter character;
    public void InitTextObj()
    {
        cooldownText = GameObject.Find("AbilityCDText").GetComponent<TextMeshProUGUI>();
        costText = GameObject.Find("AbilityCostText").GetComponent <TextMeshProUGUI>();
        UpdateTextDisplay();
    }
    private void UpdateTextDisplay()
    {
        cooldownText.text = CooldownText();
        costText.text = $"$:{cost}";
    }
    private string CooldownText()
    {
        if(cooldown >= 0 && cooldown != abilityCooldown)
        {
            return cooldown.ToString();
        }
        else
        {
            return "Ready";
        }
    }
    public virtual void ActivateAbility()
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
        character.UpdateDisplays_Public();
    }
    private void SpendMana()
    {
        character.stats.CurrentMana -= cost;
        character.CheckState();
    }
    private void SetCooldown()
    {
        cooldown = abilityCooldown;
    }
    public virtual void ActivateAbility(EnemyCharacter target)
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
        UpdateTextDisplay();
    }
    public virtual void ActivateAbility(BaseCharacter target)
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
        UpdateTextDisplay();
    }
    public virtual void ActivateAbility(int damage)
    {
        ChangeButtonEnable(false);
        SetCooldown();
        SpendMana();
        UpdateTextDisplay();
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
        UpdateTextDisplay();
    }
    public void ChangeButtonEnable(bool value)
    {
        character.abilityButton.enabled = value;
        character.abilityButton.interactable = value;
    }
    public virtual string GetDescription()
    {
        return $"does things";
    }
}
