using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDisplayManager : MonoBehaviour
{
    Vector2[] oneMember = { new Vector2(0, 0) };
    Vector2[] twoMembers = { new Vector2(0, 0), new Vector2(0, -150) }; 
    Vector2[] threeMembers = { new Vector2(0, 0), new Vector2(0, -150), new Vector2(0, -300) };
    Vector2[] fourMembers = { new Vector2(0, 0), new Vector2(0, -150), new Vector2(0, -300), new Vector2(0, -450) }; 
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
            party.members.Add(characterObj.GetComponent<Character>());
            RectTransform rect = characterObj.GetComponent<RectTransform>();
            rect.anchoredPosition = positions[i];
            rect.SetParent(this.gameObject.transform,false);
        }
    }
}
