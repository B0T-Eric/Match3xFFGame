using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestCharacter : MonoBehaviour
{
    BaseCharacter character;
    Image characterSprite;
    Button b;
    RestManager restManager;
    public void DisplayCharacter(BaseCharacter c)
    {
        character = c;
        restManager = GameObject.Find("RestSceneHud").GetComponent<RestManager>();
        characterSprite = GetComponent<Image>();
        b = GetComponent<Button>();
        b.onClick.AddListener(Select);
        
        characterSprite.sprite = character.sprite;
    }
    void Select()
    {
        restManager.SelectCharacter(character);
        
    }
}
