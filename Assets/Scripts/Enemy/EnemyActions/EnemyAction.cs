using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    public Sprite sprite;
    public virtual void PreformAction(BaseCharacter target)
    {

    }
    public virtual void PreformAction(List<BaseCharacter> targets)
    {

    }
    public virtual void PreformAction(EnemyCharacter target)
    {

    }
    public virtual void PreformAction(List<EnemyCharacter> targets)
    {

    }
}
