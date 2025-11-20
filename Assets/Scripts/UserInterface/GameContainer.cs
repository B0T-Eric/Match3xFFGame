using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class GameContainer : MonoBehaviour
{
    public static GameContainer Instance;
    public GameObject EnemyPrefab;
    public List<Transform> EnemyTransforms;
    public List<GameObject> displays;
    public List<InGameCharacterDisplay> enemyDisplays;
    List<InGameCharacterDisplay> characterDisplays = new List<InGameCharacterDisplay>(4);
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

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
        if(character is WolfEnemy wolves)
        {
            for(int i = 0; i < wolves.PackSize; i++)
            {
                GameObject wolfDisplayObj = Instantiate(EnemyPrefab, EnemyTransforms[i]);
                wolfDisplayObj.transform.SetParent(GameObject.Find("GameContainer").transform);
                InGameCharacterDisplay wolfDisplay = wolfDisplayObj.GetComponent<InGameCharacterDisplay>();
                wolfDisplay.DisplayCharacter(character);
                enemyDisplays.Add(wolfDisplay);
            } 
        }
        else
        {
            GameObject enemyDisplayObj = Instantiate(EnemyPrefab, EnemyTransforms[0]);
            enemyDisplayObj.transform.SetParent(GameObject.Find("GameContainer").transform);
            InGameCharacterDisplay enemyDisplay = enemyDisplayObj.GetComponent<InGameCharacterDisplay>();
            enemyDisplay.DisplayCharacter(character);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
