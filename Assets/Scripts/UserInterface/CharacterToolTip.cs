public class CharacterToolTip : ToolTip
{
    protected BaseCharacter character;
    private void Start()
    {
        character = transform.parent.parent.parent.GetComponent<BaseCharacter>();
        Tooltip = character.description;
    }
}
