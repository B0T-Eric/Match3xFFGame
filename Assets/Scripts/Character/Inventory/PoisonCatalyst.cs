using System.Collections;
using System.Linq;
using UnityEngine;


public class PoisonCatalyst : DraggableItem, IOnAttackItem
{
    public void OnAttack(BaseCharacter character, int damage)
    {
        if(damage >= 20)
        {
            var poison = EnemyCharacter.Instance?.statusEffects?.OfType<Poison>().FirstOrDefault();
            if (poison != null)
            {
                EnemyCharacter.Instance.LoseMana(poison.StackCount * 3);
            }
        }
    }
}
