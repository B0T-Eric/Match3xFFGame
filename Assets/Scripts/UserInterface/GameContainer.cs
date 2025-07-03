using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContainer : MonoBehaviour
{
    public static GameContainer Instance;
    public List<GameObject> displays;
    public InGameCharacterDisplay enemyDisplay;
    List<InGameCharacterDisplay> characterDisplays = new List<InGameCharacterDisplay>(4);
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < displays.Count; i++)
        {
            characterDisplays.Add(displays[i].GetComponent<InGameCharacterDisplay>());
        }
    }
    public void DisplayParty(List<BaseCharacter> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characterDisplays[i].DisplayCharacter(characters[i]);
        }
        int index = 0;
        foreach (InGameCharacterDisplay characterDisplay in characterDisplays)
        {
            if (!characterDisplay.IsDisplaying)
            {
                displays[index].SetActive(false);
            }
            index++;
        }
    }
    public void DisplayEnemy(BaseCharacter character)
    {
        enemyDisplay.DisplayCharacter(character);
    }

}
