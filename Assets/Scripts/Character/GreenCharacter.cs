using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCharacter : BaseCharacter
{
    public override void Init()
    {
        manaType = ManaType.Green;
        hexColour = "1CFF00";
        base.Init();
    }
}
