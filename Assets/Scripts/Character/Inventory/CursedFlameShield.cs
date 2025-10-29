using System.Collections;
using UnityEngine;

public class CursedFlameShield : DraggableItem, IOnDamageTakenItem
{
    [SerializeField]
    Sprite burnSprite;
    public void OnDamageTaken(BaseCharacter character)
    {
        Burn b = new Burn(2, 3, character, burnSprite);
        character.AddStatusEffect(b);
    }
}