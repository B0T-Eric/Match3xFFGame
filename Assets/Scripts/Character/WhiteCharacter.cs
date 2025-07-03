using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCharacter : BaseCharacter
{
    public override void GainPotential(int amount)
    {
        if(amount == 4)
        {
            amount += 3;
        }
        else if(amount > 4)
        {
            amount += 6;
        }
        else
        {
            potential += amount;
        }
        UpdateDisplays();
    }
    public override void Init()
    {
        manaType = ManaType.White;
        hexColour = "FFFFFF";
        base.Init();
    }
}
