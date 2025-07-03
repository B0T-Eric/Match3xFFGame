using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterDisplayManager : MonoBehaviour
{
    Vector2[] oneMember = { new Vector2(0, 0) };
    Vector2[] twoMembers = { new Vector2(0, 0), new Vector2(0, -150) }; 
    Vector2[] threeMembers = { new Vector2(0, 0), new Vector2(0, -150), new Vector2(0, -300) };
    Vector2[] fourMembers = { new Vector2(0, 400), new Vector2(0, 200), new Vector2(0, 0), new Vector2(0, -200) }; 
    Party party;
    
    // Start is called before the first frame update
    void Start()
    {
        party = GameObject.Find("PartyManager").GetComponent<Party>();
    }
    public void DisplayParty()
    {
        switch (party.selectedCharacters.Count) 
        {
            case 1:
                DisplayCharacters(oneMember);
                break;
            case 2:
                DisplayCharacters(twoMembers);
                break;
            case 3:
                DisplayCharacters(threeMembers);
                break;
            case 4:
                DisplayCharacters(fourMembers);
                break;
            default:
                Debug.Log("Not Enough Boys to be real");
                break;
        }
    }
    private void DisplayCharacters(Vector2[] positions)
    {
        for (int i = 0; i < party.selectedCharacters.Count; i++)
        {
            GameObject characterObj = Instantiate(party.selectedCharacters[i]);
            BaseCharacter bc = characterObj.GetComponent<BaseCharacter>();
            GameObject g = Instantiate(bc.partyUi);
            RectTransform r = characterObj.GetComponent<RectTransform>();
            g.transform.SetParent(r, false);
            bc.Init();
            party.members.Add(bc);
            if (bc.GetType() == typeof(YellowCharacter))
            {
                Party.Instance.HasKnowledgeOfEnemy = true;
            }
            r.anchoredPosition = positions[i];
            r.SetParent(this.gameObject.transform,false);
        }
    }
}
