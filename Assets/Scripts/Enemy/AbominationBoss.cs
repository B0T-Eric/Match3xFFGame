using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AbominationBoss : EnemyCharacter
{
    private BossIndicator Indicator;
    [SerializeField]
    private Sprite WoundSprite;
    private Wound wound;
    [SerializeField]
    private Sprite HungerSprite;
    private Hunger hunger;
    [SerializeField]
    private List<Sprite> AttackCycleIcons;

    private int cycleIndex = 0;
    public override void Init()
    {
        base.Init();
        hunger = new Hunger(HungerSprite, 1);
        wound = new Wound(WoundSprite, 3);
    }
    protected override void InitializeDisplays()
    {
        base.InitializeDisplays();
        Indicator = GameObject.Find("BossTurnIndicator").GetComponent<BossIndicator>();
        Indicator.SetIconImages(AttackCycleIcons);
    }
    public override void Attack(BaseCharacter c)
    {
        c.LoseMana(currentTurnDamage, this.GetType().Name);
    }
    protected override int GetDamage()
    {
        int hungerDamageAmp = GetHungerAmp();
        return maxDamage + hungerDamageAmp;
    }
    private int GetHungerAmp()
    {
        var hunger = System.Linq.Enumerable.FirstOrDefault(
            System.Linq.Enumerable.OfType<Hunger>(statusEffects)
        );
        return hunger != null ? hunger.StackCount : 0;
    }
    protected override void SelectTargets()
    {
        List<BaseCharacter> validTargets = new List<BaseCharacter>();
        foreach (BaseCharacter bc in Party.Instance.members)
        {
            if (bc.state == CharacterState.Alive || bc.state == CharacterState.Down)
            {
                validTargets.Add(bc);
            }
        }
        targets = validTargets;
    }
    public override async Task<bool> PreformTurn()
    {
        if (cycleIndex == 0)
        {
            foreach (BaseCharacter target in targets)
            {
                Attack(target);
            }
            cycleIndex = 1;
            
        }
        else if (cycleIndex == 1)
        {
            AddStatusEffect(hunger);
            cycleIndex = 2;
        }
        else if(cycleIndex == 2)
        {
            foreach(BaseCharacter target in targets)
            {
                target.AddStatusEffect(wound);
            }
            cycleIndex = 0;
        }
        Indicator.ShiftPointer();
        //to do: introduce animation or action time variable in base character
        await Task.Delay(1000);
        return true;
    }
}
