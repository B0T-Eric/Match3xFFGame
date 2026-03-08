using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;

public class WolfEnemy : EnemyCharacter
{ 
    public int PackSize;
    [SerializeField]
    private Sprite WoundSprite;
    public List<Wolf> WolfPack;
    private int WolfIndex;
    public List<EnemyInGameDisplay> WolfDisplays;
    public override void Init()
    {
        
        InitializeStats();
        maxMana = RoundToNearestMultipleOfFour(maxMana);
        WolfPack = new List<Wolf>();
        WolfIndex = 3;
        for (int i = 0; i < PackSize; i++)
        {
            WolfPack.Add(new Wolf(((maxMana/PackSize)), WoundSprite, ref statusEffects));
            int rng = UnityEngine.Random.Range(0, 2);
            WolfPack[i].SetAttackToggle(Convert.ToBoolean(rng));
        }
        InitializeDisplays();
        //give each wolf a reference to how they're displayed
        for(int i = 0; i < PackSize; i++)
        {
            WolfPack[i].SetDisplay(WolfDisplays[i]);
        }
        UpdateDisplays();
    }
    public int RoundToNearestMultipleOfFour(int number)
    {
        int remainder = number % 4;

        if (remainder == 0)
        {
            return number; 
        }
        else if (remainder <= 2) 
        {
            return number - remainder;
        }
        else 
        {
            return number + (4 - remainder);
        }
    }
    //taking damage will cause each wolf to lose health which will affect the total mana pool of the enemy
    public override void LoseMana(int amount, string source = "Unknown")
    {
        
        Wolf target = WolfPack[WolfIndex];
        if (statusEffects.Count > 0)
        {
            foreach (BaseStatus status in statusEffects)
            {
                if (status is Brittle)
                {
                    Brittle brittle = (Brittle)status;
                    amount = Mathf.RoundToInt(amount * brittle.ApplyBrittle());
                }
            }
        }
        target.TakeDamge(amount);
        //maybe?
        currentMana -= amount;
        if (target.IsDead)
        {
            WolfIndex--;
            WolfPack.Remove(target);
            GameContainer.Instance.enemyDisplays[WolfIndex].gameObject.SetActive(false);
            GameContainer.Instance.enemyDisplays.RemoveAt(WolfIndex);
            maxMana -= target.MaxHealth;
        }
        CombatLog.Instance.AmendTextToLog($"Wolf took {amount} damage from {source}");
        UpdateDisplays();
    }
    //do not consider targets.
    protected override int GetDamage()
    {
        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        if (statusEffects.Count > 0)
        {
            foreach (BaseStatus status in statusEffects)
            {
                if (status is Weakness)
                {
                    Weakness weakness = (Weakness)status;
                    damage = Mathf.RoundToInt(damage * weakness.ApplyWeakness());
                }
            }

        }
        return damage;
    }
    //call for wolf to preform the turn which will randomly alternate attacks.
    public override async Task<bool> PreformTurn()
    {
        if (!isStunned)
        {
            foreach (Wolf wolf in WolfPack)
            {
                //to do: introduce animation or action time variable in base character
                await Task.Delay(250 + (250 * WolfPack.IndexOf(wolf)));
                wolf.PreformTurn();
            } 
        }
        return true;
    }
    public override void TurnStart()
    {
        base.TurnStart();
        foreach (Wolf wolf in WolfPack)
        {
            wolf.SetTurnDamage(currentTurnDamage);

        }
    }
    protected override void SelectTargets()
    {
        List<BaseCharacter> validTargets = new List<BaseCharacter>();
        foreach(BaseCharacter bc in Party.Instance.members)
        {
            if(bc.state == CharacterState.Alive || bc.state == CharacterState.Down)
            {
                validTargets.Add(bc);
            }
        }
        foreach (Wolf wolf in WolfPack)
        {
            int targetIndex = UnityEngine.Random.Range(0,validTargets.Count);
            wolf.SetTarget(validTargets[targetIndex]);
        }

    }
    public override int GetDamage_Public()
    {
        int damage = 0;
        foreach(Wolf w in WolfPack)
        {
            damage += w.GetDamage();
        }
        return damage / PackSize;
    }

}
public class Wolf
{
    private Wound WoundStatus;
    private int TurnDamage;
    public int MaxHealth;
    private int CurrentHealth;
    public bool IsDead;
    public BaseCharacter target;
    private EnemyInGameDisplay display;
    private bool AttackToggle;
    public void Attack()
    {
        target.LoseMana(TurnDamage, "Wolf");
    }
    public void SetAttackToggle(bool b)
    {
        AttackToggle = b;
    }
    public void SetTurnDamage(int i)
    {
        TurnDamage = i;
    }
    public void Wound()
    {
        target.LoseMana(TurnDamage,"Wolf");
        target.AddStatusEffect(WoundStatus);
    }
    public int GetDamage()
    {
        return TurnDamage;
    }
    public void PreformTurn()
    {
        display.AttackTrue();
        if (AttackToggle)
        {
            Attack();
            AttackToggle = !AttackToggle;
        }
        else
        {
            Wound();
            AttackToggle = !AttackToggle;
        }
    }
    public void SetTarget(BaseCharacter c)
    {
        target = c;
    }
    public void TakeDamge(int damage)
    {
        display.DefendTrue();
        CurrentHealth -= damage;
        
        if (CurrentHealth <= 0)
        {
            IsDead = true;
        }

    }
    public void SetDisplay(EnemyInGameDisplay dis)
    {
        display = dis;
    }
    public Wolf(int maxHealth, Sprite woundIcon, ref ObservableCollection<BaseStatus> statusEffects)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        IsDead = false;
        int woundDuration = 2;
        WoundStatus = new Wound(woundIcon,woundDuration);
    }
}
