using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationItem : MonoBehaviour
{
    public int RowNumber;
    public int ColumnIndex;
    public GameObject MapPrefab;
    public MapItem ItemType;
    [SerializeField]
    private List<NavigationItem> belowConnections;
    [SerializeField]
    private List<NavigationItem> aboveConnections;
    public bool PlayerIsHere = false;
    bool playerCanMoveTo = false;
    public bool IsCleared { get; private set; } = false;
    private Button itemButton;
    Image backgroundImage;
    private void Start()
    {
        itemButton = gameObject.GetComponent<Button>();   
        if(belowConnections == null && aboveConnections == null)
        {
            belowConnections = new();
            aboveConnections = new();
        }
        
    }
    public int GetAboveConnectionCount() => aboveConnections.Count;
    public List<NavigationItem> GetAboveConnections() => aboveConnections;
    public int GetBelowConnectionCount() => belowConnections.Count;
    public List<NavigationItem> GetBelowConnections() => belowConnections;

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
    //look through row below to find connections, which determines the validity of the navItem.
    public void MakeConnections(ref GameObject[][] navGrid)
    {
        int belowIndex = RowNumber -1;
        int minCol = ColumnIndex - 1;
        int maxCol = ColumnIndex + 1;
        if(belowIndex >= 0)
        {
            GameObject[] rowBelow = navGrid[belowIndex]; 
            for (int col = minCol; col <= maxCol; col++)
            {
                if(col < 0) continue;
                if (col < rowBelow.Length)
                {
                    GameObject colItem = rowBelow[col];
                    if (colItem != null)
                    {
                        NavigationItem nav = colItem.GetComponent<NavigationItem>();
                        if (!nav.aboveConnections.Contains(this))
                        {
                            nav.AddAboveConnection(this);
                        }
                        if (!belowConnections.Contains(nav))
                        {
                            AddBelowConnection(nav);
                        }
                        
                        
                    }
                }
            }
        }
    }
    public void AddBelowConnection(NavigationItem item)
    {
        belowConnections.Add(item);
    }
    public void AddAboveConnection(NavigationItem item)
    {
        aboveConnections.Add(item);
    }
    public void RemoveBelowConnection(NavigationItem item)
    {
        belowConnections.Remove(item);
    }
    public void RemoveAboveConnection(NavigationItem item)
    {
        aboveConnections.Remove(item);
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
            if (IsCleared)
            {
                if (ColorUtility.TryParseHtmlString("#631461", out Color purple))
                    backgroundImage.color = purple;
                else
                    backgroundImage.color = new Color(99f/255f, 20f/255f, 97f/255f, 1f);
            }
            else
            {
                backgroundImage.color = Color.white;
            }
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
    public void RemoveItemFromMap()
    {
        Destroy(gameObject);
    }
    public void ToggleButton(bool b)
    {
        itemButton.interactable = b;
        playerCanMoveTo = b;
    }
    public void OnClicked()
    {
        MapController.Instance.currentLocation.PlayerIsHere = false;
        switch (ItemType)
        {
            case MapItem.Normal:
                LoadEnemyScene();
                break;
            case MapItem.Mystery:
                LoadRandomScene();
                break;
            case MapItem.Rest:
                LoadRestScene();
                break;
            case MapItem.Boss:
                LoadBossScene();
                break;
            default:
                LoadRandomScene();
                break;
        }
        MapController.Instance.SetCurrentPosition(this);
        PlayerIsHere = true;
    }
}
