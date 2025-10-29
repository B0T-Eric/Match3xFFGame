using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class RestManager : MonoBehaviour
{
    [SerializeField]
    int partyHealAmount;
    [SerializeField]
    int soloHealAmount;
    BaseCharacter selectedCharacter;
    public Button continueButton,healButton,singleButton;
    [SerializeField]
    GameObject selectCharObj;
    List<GameObject> charObjects = new List<GameObject>();
    private void Start()
    {
        continueButton.enabled = false;
        foreach (BaseCharacter character in Party.Instance.members)
        {
            GameObject rcObj = Instantiate(selectCharObj,transform,false);
            charObjects.Add(rcObj);
            RestCharacter rc = rcObj.GetComponent<RestCharacter>();
            rc.DisplayCharacter(character);
        }
    }
    public void PartyHeal()
    {
        foreach(BaseCharacter character in Party.Instance.members)
        {
            character.HealMana(partyHealAmount);
            EmberOfGraceUpgrade(character);

        }
        DisableOptions();
    }

    private static void EmberOfGraceUpgrade(BaseCharacter character)
    {
        foreach (DraggableItem item in character.items)
        {
            if (item is EmberOfGrace e)
            {
                e.IncreaseRestCount();
            }
        }
    }

    public void HealMember()
    {
        if (selectedCharacter == null) return;
        else
        {
            selectedCharacter.HealMana(soloHealAmount);
            EmberOfGraceUpgrade(selectedCharacter);
            DisableOptions();
        }
    }
    private void DisableOptions()
    {
        for(int i = 0; i < charObjects.Count; i++)
        {
            charObjects[i].SetActive(false);
        }
        healButton.gameObject.SetActive(false);
        singleButton.gameObject.SetActive(false);
        continueButton.enabled = true;
    }
    public void SelectCharacter(BaseCharacter c)
    {
        selectedCharacter = c;
    }
}
