using System.Collections;
using TMPro;
using UnityEngine;
public class EmberOfGrace : DraggableItem, IStatItem
{
    [SerializeField]
    TextMeshProUGUI restText;
    public int RestCount = 0;
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
        itemSO.healingMult += (float)(num * 1.5f) / 10f;
    }
    public void DecreaseStats(int num)
    {
        itemSO.healingMult -= (float)(num * 1.5f) / 10f;
    }
    public void OnEquip()
    {
        IncreaseStats(RestCount);
    }

    public void OnUnequip()
    {
        DecreaseStats(RestCount);
    }
}