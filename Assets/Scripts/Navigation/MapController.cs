using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    private static MapController instance;
    public static MapController Instance { get { return instance; } }

    public Canvas MapCanvas;
    public int CurrentStage = 0;
    //all possible enemy prefabs to include in scenes.
    public GameObject[] EnemyArray;
    public GameObject[] BossArray;
    //a set of enemies, 1 per row for the game to pull from if required
    public GameObject[] EnemyList;
    //increment stage count at end of stage.
    public int stageCount = 0;
    public float EnemyHealthModifier = 1f;
    public float EnemyDamageModifier = 1f;

    // Selection tracking
    public int SelectedRow { get; private set; } = -1;
    public int SelectedColumn { get; private set; } = -1;
    public GameObject SelectedMapPrefab { get; private set; }
    public NavigationItem.MapItemType SelectedType { get; private set; } = NavigationItem.MapItemType.Unknown;

    // Cleared rooms tracking (persisting across scene loads)
    private HashSet<string> clearedRooms = new HashSet<string>();
    //2D list to hold each rows items.
    [SerializeField]
    private int LevelMapCount;
    bool hasInitialized = false;
    private List<List<GameObject>> LevelMap;
    //Items to place on map screen (buttons which contain either normal encounter, boss ecounter (at end), random encounters, or resting scene
    //index 0 = normal, 1 = random, 2 = rest, 3 = boss.
    [SerializeField]
    private GameObject[] mapItems;
    //placement offset to x vector 2 value
    [SerializeField]
    int xOffset;
    //amount of rows in the map.
    [SerializeField]
    int rowCount;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    public GameObject GetRandomBoss()
    {
        return BossArray[Random.Range(0, BossArray.Length - 1)];
    }
    private void Start()
    { 
        if (!hasInitialized)
        {
            LevelMap = new List<List<GameObject>>();
            EnemyList = new GameObject[rowCount];
            GenerateMap();
            hasInitialized = true;
            LevelMapCount = LevelMap.Count();
        }
    }

    /// <summary>
    /// Record the room the player selected. This persists across scene loads (MapController is DontDestroyOnLoad).
    /// </summary>
    public void SelectRoom(int row, int col, GameObject mapPrefab, NavigationItem.MapItemType type)
    {
        SelectedRow = row;
        SelectedColumn = col;
        SelectedMapPrefab = mapPrefab;
        SelectedType = type;
        Debug.Log($"MapController.SelectRoom row={row} col={col} type={type} prefab={(mapPrefab != null ? mapPrefab.name : "null")}");
    }

    /// <summary>
    /// Mark a room as cleared so it can be displayed differently in the UI.
    /// </summary>
    public void MarkRoomCleared(int row, int col)
    {
        if (row < 0 || col < 0) return;
        clearedRooms.Add(RoomKey(row, col));
    }

    public bool IsRoomCleared(int row, int col)
    {
        if (row < 0 || col < 0) return false;
        return clearedRooms.Contains(RoomKey(row, col));
    }

    private string RoomKey(int row, int col) => $"{row}_{col}";

    /// <summary>
    /// Populate ui canvas with the buttons for navigation to specific 
    /// encounters.
    /// Rows will contain 2-4 different items, each item can be either a enemy, unknown or rest encounter.
    /// 
    /// </summary>
    public void GenerateMap()
    {
        for (int i = 0; i < rowCount; i++)
        {
            EnemyList[i] = EnemyArray[UnityEngine.Random.Range(0, EnemyArray.Length)];
            GameObject[] rowObjects = null;
            if (i > 0 && i < rowCount-1)
            {
                //In each row randomise items, having 1-5 different rooms in each
                int rowItems = UnityEngine.Random.Range(2, 5);
                rowObjects = new GameObject[rowItems];
                if (rowObjects != null)
                {
                    //initialize each rowObject item
                    for (int j = 0; j < rowObjects.Count(); j++)
                    {
                        int rng = UnityEngine.Random.Range(1, 101);
                        if(rng > 0 && rng < 60)
                        {
                            //enemy
                            rowObjects[j] = mapItems[0];
                            
                        }
                        else if(rng >60 && rng < 85)
                        {
                            //bonus
                            rowObjects[j] = mapItems[1];
                        }
                        else
                        {
                            //rest
                            rowObjects[j] = mapItems[2];
                            
                        }
                            
                    }
                }
            }
            else if(i == 0)
            {
                rowObjects = new GameObject[1];
                rowObjects[0] = mapItems[0];
            }
            else if(i == rowCount - 1)
            {
                rowObjects = new GameObject[1];
                rowObjects[0] = mapItems[3];
            }
            
            List<GameObject> completeRow = rowObjects.ToList();
            LevelMap.Add(completeRow);
        }
        PopulateUI(rowCount);
    }
    /// <summary>
    /// Chat GPT schizophrenia - used it to help style.
    /// </summary>
    /// <param name="rowCount"></param>
    private void PopulateUI(int rowCount)
    {
        float padding = 100f;
        RectTransform canvasRect = MapCanvas.GetComponent<RectTransform>();
        float canvasHeight = canvasRect.rect.height;
        float canvasWidth = canvasRect.rect.width;
        float usableHeight = canvasHeight - (2 * padding);
        float yOffset = rowCount > 1 ? usableHeight / (rowCount - 1) : 0;
        float startY = canvasHeight / 2f - padding;
        for (int yCount = 0; yCount < rowCount; yCount++)
        {
            List<GameObject> row = LevelMap[yCount];
            
            float y = - startY + (yCount * yOffset);
            
            float xStart = -((row.Count - 1) * xOffset) / 2f;
            for (int xCount = 0; xCount < row.Count; xCount++)
            {
                float x = xStart + (xCount * xOffset);
                Vector2 anchoredPos = new Vector2(x, y);

                // NOTE: row[xCount] is the prefab; g is the instantiated visual object.
                GameObject prefabForCell = row[xCount];
                GameObject g = Instantiate(prefabForCell, MapCanvas.transform);
                NavigationItem navItem = g.GetComponent<NavigationItem>();
                if (navItem != null)
                {
                    UnityEngine.UI.Button b = g.GetComponent<UnityEngine.UI.Button>();
                    navItem.StageNumber = yCount;
                    // column index and prefab + type for the nav item
                    navItem.ColumnIndex = xCount;
                    navItem.MapPrefab = prefabForCell;
                    navItem.ItemType = GetItemTypeFromPrefab(prefabForCell);

                    // set cleared state on the nav item
                    navItem.SetCleared(IsRoomCleared(yCount, xCount));

                    // ensure clicking the button records the selection and loads at once
                    if (b != null)
                    {
                        b.onClick.RemoveAllListeners();
                        b.onClick.AddListener(navItem.OnClicked);
                        b.interactable = (navItem.StageNumber == CurrentStage);
                        ColorBlock cb = b.colors;
                        cb.normalColor = (navItem.StageNumber == CurrentStage) ? Color.white : Color.gray;
                        b.colors = cb;
                    }
                }
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchoredPosition = anchoredPos;
            }
        }
    }

    // small helper to map the prefab reference to a friendly enum
    private NavigationItem.MapItemType GetItemTypeFromPrefab(GameObject prefab)
    {
        if (prefab == null) return NavigationItem.MapItemType.Unknown;
        if (mapItems != null && mapItems.Length > 0)
        {
            if (prefab == mapItems[0]) return NavigationItem.MapItemType.Enemy;
            if (mapItems.Length > 1 && prefab == mapItems[1]) return NavigationItem.MapItemType.Bonus;
            if (mapItems.Length > 2 && prefab == mapItems[2]) return NavigationItem.MapItemType.Rest;
            if (mapItems.Length > 3 && prefab == mapItems[3]) return NavigationItem.MapItemType.Boss;
        }
        return NavigationItem.MapItemType.Unknown;
    }

    public void ChangeEnable()
    {
        int stageAfter = CurrentStage;
        foreach (List<GameObject> gameObjects in LevelMap)
        {
            foreach (GameObject g in gameObjects)
            {
                NavigationItem item = g.GetComponent<NavigationItem>();
                UnityEngine.UI.Button b = g.GetComponent<UnityEngine.UI.Button>();
                if (item != null && b != null)
                {
                    // Only enable the buttons for the next stage
                    b.interactable = (item.StageNumber == stageAfter);
                    ColorBlock cb = b.colors;
                    cb.normalColor = (item.StageNumber == stageAfter) ? Color.white : Color.gray;
                    b.colors = cb;
                }
            }
        }
    }
    public void IncrementStage()
    {
        CurrentStage++;
        EnemyHealthModifier += 0.1f;
        EnemyDamageModifier += 0.05f;
        Party.Instance.StageText.text = $"Stage: {CurrentStage + 1}";
    }
    public void LoadMapScene()
    {
        StartCoroutine(LoadAndPopulate());
    }

    private IEnumerator LoadAndPopulate()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("NavigationScene");

        while (!asyncLoad.isDone)
            yield return null;

        yield return null;

        if (MapCanvas == null)
        {
            MapCanvas = GameObject.Find("MapCanvas")?.GetComponent<Canvas>();
        }

        if (MapCanvas != null)
        {
            PopulateUI(rowCount);
        }
    }

}
