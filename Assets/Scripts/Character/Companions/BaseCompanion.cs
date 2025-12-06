using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
/// <summary>
/// Foundation for companions which preform an action every turn alongside their assigned character.
/// </summary>
public abstract class BaseCompanion : MonoBehaviour
{
    protected BaseCharacter pairedCharacter;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    protected string companionName;
    public virtual void Init(BaseCharacter c)
    {
        pairedCharacter = c;
    }
    public Sprite GetSprite() { return sprite; }
    public abstract void PreformAction();
    public string GetCompanionToolTip()
    {
        return $"<b>{companionName}</b>" +
            $"\n{GetDescription()}";
    }
    protected virtual string GetDescription()
    {
        return $"Does things each turn";
    }

}
