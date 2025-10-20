using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationItem : MonoBehaviour
{
    public enum MapItemType
    {
        Enemy,
        Bonus,
        Rest,
        Boss,
        Unknown
    }

    public int StageNumber;
    // column index within the stage/row
    public int ColumnIndex;
    // prefab reference that this button represents
    public GameObject MapPrefab;
    // friendly type for scene selection
    public MapItemType ItemType = MapItemType.Unknown;

    // cleared UI state
    public bool IsCleared { get; private set; } = false;

    // optional: assign an Image on the prefab or use the root Image
    Image backgroundImage;

    private void Awake()
    {
        // try to get an Image component on the same GameObject
        backgroundImage = GetComponent<Image>();
        // if not found, try to find first Image child
        if (backgroundImage == null)
        {
            backgroundImage = GetComponentInChildren<Image>(true);
        }
    }

    public void SetCleared(bool cleared)
    {
        IsCleared = cleared;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (backgroundImage != null)
        {
            // cleared rooms get a subtle green tint; adjust to taste
            backgroundImage.color = IsCleared ? new Color(0.75f, 0.9f, 0.75f) : Color.white;
        }
    }

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
        SceneManager.LoadSceneAsync("BonusScene");

    }
    public void OnClicked()
    {
        if (MapController.Instance != null)
        {
            MapController.Instance.SelectRoom(StageNumber, ColumnIndex, MapPrefab, ItemType);
        }
        switch (ItemType)
        {
            case MapItemType.Enemy:
                LoadEnemyScene();
                break;
            case MapItemType.Bonus:
                LoadRandomScene();
                break;
            case MapItemType.Rest:
                LoadRestScene();
                break;
            case MapItemType.Boss:
                LoadBossScene();
                break;
            default:
                LoadRandomScene();
                break;
        }
    }
}
