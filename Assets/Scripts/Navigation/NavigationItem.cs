using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationItem : MonoBehaviour
{
    public void LoadEnemyScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadRestScene()
    {
        SceneManager.LoadScene("RestScene");
    }
    public void LoadBossScene()
    {
        SceneManager.LoadSceneAsync("BossScene");
    }
    /// <summary>
    /// Randomly determine whether to load an enemy, shop, bonus room or something else (tbd).
    /// </summary>
    public void LoadRandomScene()
    {

    }
}
