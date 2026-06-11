using Assets.Scripts.UserInterface;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class GameContainer : MonoBehaviour
{
    public static GameContainer Instance;
    public GameObject EnemyPrefab;
    public List<Transform> EnemyTransforms;
    public List<Transform> PartyTransforms;
    public List<InGameCharacterDisplay> enemyDisplays;
    List<InGameCharacterDisplay> characterDisplays = new List<InGameCharacterDisplay>(4);
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void DisplayParty(List<BaseCharacter> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            GameObject g = Instantiate(characters[i].InGameDisplayPrefab, PartyTransforms[i]);
            characterDisplays.Add(g.GetComponent<InGameCharacterDisplay>());
            characterDisplays[i].DisplayCharacter(characters[i]);
        }
    }
    public void DisplayEnemy(EnemyCharacter character)
    {
        if(character is WolfEnemy wolves)
        {
            WolfEnemy we = character as WolfEnemy;
            for(int i = 0; i < wolves.PackSize; i++)
            {
                GameObject wolfDisplayObj = Instantiate(EnemyPrefab, EnemyTransforms[i]);
                wolfDisplayObj.transform.SetParent(GameObject.Find("GameContainer").transform);
                EnemyInGameDisplay wolfDisplay = wolfDisplayObj.GetComponent<EnemyInGameDisplay>();
                we.WolfDisplays.Add(wolfDisplay);
                wolfDisplay.DisplayCharacter(character);
                enemyDisplays.Add(wolfDisplay);
                
            } 
        }
        else
        {
            GameObject enemyDisplayObj = Instantiate(EnemyPrefab, EnemyTransforms[0]);
            enemyDisplayObj.transform.SetParent(GameObject.Find("GameContainer").transform);
            EnemyInGameDisplay enemyDisplay = enemyDisplayObj.GetComponent<EnemyInGameDisplay>();
            enemyDisplay.DisplayCharacter(character);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
