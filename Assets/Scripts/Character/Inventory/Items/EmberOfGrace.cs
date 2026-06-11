using System.Collections;
using TMPro;
using UnityEngine;
public class EmberOfGrace : BaseItem, IStatItem
{
    [SerializeField]
    TextMeshProUGUI restText;
    public int RestCount = 0;
    public float healModifier = 1.5f;
    private void Start()
    {
        restText.text = RestCount.ToString();
    }
    public void IncreaseRestCount()
    {
        RestCount++;
        IncreaseStats(1);
        restText.text = RestCount.ToString();
    }
    public void IncreaseStats(int num)
    {
        itemSO.healingMult += (float)(num * healModifier) / 10f;
    }
    public void DecreaseStats(int num)
    {
        itemSO.healingMult -= (float)(num * healModifier) / 10f;
    }
    public void OnEquip()
    {
        IncreaseStats(RestCount);
    }

    public void OnUnequip()
    {
        DecreaseStats(RestCount);
    }
    public override string GetEffectDesc()
    {
        return $"<b>Current Stacks: {RestCount}</b>\nEvery time you rest this item gains stacking increases (+{healModifier * 10}%) to healing.";
    }
}