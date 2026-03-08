using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttackOption : MonoBehaviour
{
    private Button button;
    private int index = 0;
    private BaseCharacter character;
    private TextMeshProUGUI buttonText;
    private ToolTip toolTip;
    public void Init(int attackIndex,string attack,string desc,BaseCharacter c)
    {
        toolTip = GetComponent<ToolTip>();
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        character = c;
        button.onClick.AddListener(SetCharacterAttack);
        index = attackIndex;
        buttonText.text = attack;
        button.onClick.AddListener(HideTooltipOnClick);
        toolTip.SetToolTip(desc);
    }
    private void SetCharacterAttack()
    {
        character.selectedAttack = index;
    }
    private void HideTooltipOnClick()
    {
        HoverToolTip.HideTootTip_Static();
    }
}
