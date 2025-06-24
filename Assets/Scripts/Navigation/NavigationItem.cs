using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationItem : MonoBehaviour
{
    public int StageNumber;
    private int nextStageNumber;
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
        int scenePicker = Random.Range(0, 3);
        switch (scenePicker)
        {
            case 0:
                LoadEnemyScene();
                break;
            case 1:
                LoadRestScene();
                break;
            case 2:
                SceneManager.LoadSceneAsync("BonusScene");
                break;
        }
    }
}
