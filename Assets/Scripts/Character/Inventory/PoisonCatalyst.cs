using System.Collections;
using System.Linq;
using UnityEngine;


public class PoisonCatalyst : DraggableItem, IOnAttackItem
{
    private int damageMultiplier = 3;
    private int damageThreshold = 20;
    public void OnAttack(BaseCharacter character, int damage)
    {
        if(damage >= damageThreshold)
        {
            var poison = EnemyCharacter.Instance?.statusEffects?.OfType<Poison>().FirstOrDefault();
            if (poison != null)
            {
                EnemyCharacter.Instance.LoseMana(poison.StackCount * damageMultiplier);
            }
        }
    }
    public override string GetEffectDesc()
    {
        return $"When damage dealt by the character who currently has this item equiped exceeds {damageThreshold} and the enemy is poisoned,\n deal {damageMultiplier} x poison stacks as additional damage.";
    }
}
