using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public interface IReward
{
    void ApplyReward();
}
public class ExchangeReward : IReward
{
    Sprite icon;
    ManaType type;
    public ExchangeReward(Sprite s, ManaType type)
    {
        icon = s;
    }
    public void ApplyReward()
    {
        Party.Instance.statuses.Add(new ExchangeEffect(icon,Party.Instance.members,type));
    }
}
public class ItemReward : IReward
{
    private string itemId;
    public ItemReward(string id) { itemId = id; }
    public void ApplyReward()
    {
        Debug.Log($"Player received item: {itemId}");
        // InventoryManager.Instance.AddItem(itemId);
    }
}

public class UpgradeReward : IReward
{
    private string upgradeId;
    public UpgradeReward(string id) { upgradeId = id; }
    public void ApplyReward()
    {
        Debug.Log($"Player upgraded: {upgradeId}");
        // Upgrade logic...
    }
}
public class PartyStatusReward : IReward
{
    private BaseStatus partyStatus;
    public PartyStatusReward(BaseStatus partyStatus) { this.partyStatus = partyStatus; }
    public void ApplyReward()
    {
        //add status to party.

    }
}

public class EvolutionReward : IReward
{
    private BaseCharacter character;
    public EvolutionReward(BaseCharacter character) { this.character = character; }
    public void ApplyReward()
    {
        Debug.Log($"{character.name} evolved!");
        // Evolution logic...
    }
}
