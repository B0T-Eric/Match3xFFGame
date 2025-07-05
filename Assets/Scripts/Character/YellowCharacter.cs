using UnityEngine;
/// <summary>
/// 
/// </summary>
public class YellowCharacter : BaseCharacter
{
    //heal amount 
    int heal = 5;
    public Sprite weaknessIcon;
    public override void Init()
    {
        manaType = ManaType.Yellow;
        hexColour = "FFEA00";
        base.Init();
    }

    public override void Attack(BaseCharacter c)
    {
        if (currentMana > secPassiveThreshold)
        {
            ApplyWeakness(c);
        }
        base.Attack(c);

    }
    private void ApplyWeakness(BaseCharacter c) 
    {
        //Modifier = 0.75 (25% reduction), Duration = 2 turns.
        Weakness weakness = new Weakness(2, 0.75f, weaknessIcon);
        c.AddStatusEffect(weakness);
    }
    public override void TurnStart()
    {
        if (currentMana > thirdPassiveThreshold)
        {
            foreach (BaseCharacter c in Party.Instance.members)
            {
                c.HealMana(heal);
            }
        }
        base.TurnStart();
    }
}

