using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAdventure : MonoBehaviour
{
    public string scene;
    public void LoadGameScene()
    {
        SceneManager.LoadScene(scene);
        Party.Instance.ToggleText();
    }
}
