using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDisplayManager : MonoBehaviour
{ 
    Party party;
    public List<Transform> memberLocations = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        party = GameObject.Find("PartyManager").GetComponent<Party>();
    }
    public void DisplayParty()
    {
        DisplayCharacters();
    }
    private void DisplayCharacters()
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
            //r.anchoredPosition = positions[i];
            r.SetParent(memberLocations[i],false);
        }
    }
}
