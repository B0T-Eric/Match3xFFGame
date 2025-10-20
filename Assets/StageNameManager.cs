using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageNameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI stageNameText;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene previous, Scene next)
    {
        if (next.name == "NavigationScene")
        {
            stageNameText.text = "Map";
        }
        else if(next.name == "GameScene")
        {
            stageNameText.text = "Battle";
        }
        else if (next.name == "RestScene")
        {
            stageNameText.text = "Rest";
        }
        else if (next.name == "BossScene")
        {
            stageNameText.text = "Boss Battle";
        }
        else if (next.name == "BonusScene")
        {
            stageNameText.text = "Bonus Room";
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
