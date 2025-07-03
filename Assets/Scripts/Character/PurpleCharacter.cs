using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCharacter : BaseCharacter
{
    public override void Init()
    {
        manaType = ManaType.Purple;
        hexColour = "AD00FF";
        base.Init();
    }
}
