using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttack : MonoBehaviour
{
    public string Description;
    public string Name;

    public virtual void Attack(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
    {
        Hit(c, damage, origin,characterName, onAttack);
    }
    public virtual void Attack(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack, float[] modifiers)
    {

    }
    /// <summary>
    /// This overload is for applying a status to the enemy
    /// </summary>
    /// <param name="target">character being attacked</param>
    /// <param name="damage">damage being dealt</param>
    /// <param name="origin">character attacking the target</param>
    /// <param name="characterName">name of attacking character</param>
    /// <param name="onAttack">on attack delegate</param>
    /// <param name="status">status effect to be applied to enemy</param>
    public virtual void Attack(BaseCharacter target, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack, BaseStatus status)
    {

    }
    protected void Hit(BaseCharacter c, int damage, BaseCharacter origin, string characterName, OnAttackDelegate onAttack)
    {
        c.LoseMana(damage, characterName);
        //delegate for apply on hit effects/extra damage
        onAttack?.Invoke(c, damage);
        c.onHit?.Invoke(origin);
        CombatLog.Instance.AmendTextToLog($"{characterName} attacked {c.GetCharacterName()} for {damage}");
    }
}