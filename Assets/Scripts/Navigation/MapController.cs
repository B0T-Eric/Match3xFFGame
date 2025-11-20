using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum MapItem { Normal, Mystery, Rest, Boss }
public class MapController : MonoBehaviour
{
    private static MapController instance;
    public static MapController Instance { get { return instance; } }
    [SerializeField]
    private Sprite pathSpirte;
    public int stageCount = 0;
    public float EnemyHealthModifier = 1f;
    public float EnemyDamageModifier = 1f;
    public Canvas MapCanvas;
    //all possible enemy prefabs to include in scenes.
    public GameObject[] EnemyArray;
    public GameObject[] BossArray;
    //a set of enemies, 1 per row for the game to pull from if required
    public GameObject[] EnemyList;
    [SerializeField]
    private int LevelMapCount;
    bool hasInitialized = false;
    //Items to place on map screen (buttons which contain either normal encounter, boss ecounter (at end), random encounters, or resting scene
    //index 0 = normal, 1 = mystery, 2 = rest, 3 = boss.
    [SerializeField]
    private GameObject[] mapItems;
    public int CurrentStage = 0;
    #region grid
    [SerializeField]
    int rows, cols, roomsPerRow;
    const int height = 64;
    const int width = 64;
    GameObject[][] navGrid;
    List<int> currentValid = new List<int>();
    #endregion
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
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            await SceneManager.LoadSceneAsync("BossScene");
        }
    }
    public GameObject GetRandomBoss()
    {
        return BossArray[Random.Range(0, BossArray.Length - 1)];
    }

    private void Start()
    { 
        if (!hasInitialized)
        {
            EnemyList = new GameObject[rows];
            MakeGrid(rows,cols);
            hasInitialized = true;
        }
    }
    #region MapGeneration
    /// <summary>
    /// Create a grid of a set height and width that contains navigation items.
    /// Populate 2D array of navigation items within the grid, setting empty spaces as null.
    /// First item connects to all.
    /// Last item connects to all.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    private void MakeGrid(int rows, int cols)
    {
        GenerateGrid(rows, cols);
        StartCoroutine(DelayedUI());
    }

    private void GenerateGrid(int rows, int cols)
    {
        navGrid = new GameObject[rows][];
        //current valid placements for a node
        
        for (int i = 0; i < rows; i++)
        {
            EnemyList[i] = EnemyArray[Random.Range(0, EnemyArray.Length)];
            GameObject[] navItems = new GameObject[cols];
            if (i > 1 && i < rows - 2)
            {
                int roomsToPlace = Random.Range(2, 5);
                navItems = PopulateRow(cols, roomsToPlace,i); 
            }
            else if (i == 0) //start in center at bottom row
            {
                navItems[cols / 2] = GetMapItem(MapItem.Normal, i, cols / 2);
            }
            else if (i == rows - 1) //boss in center at top row
            {
                int c = cols / 2;
                navItems[c] = GetMapItem(MapItem.Boss, i, c);
            }
            else //row after start and before boss must spawn 2
            {
                int col1 = (cols / 2) + 1;
                int col2 = (cols / 2) - 1;
                navItems[col1] = GetRandomMapItem(i, col1);
                navItems[col2] = GetRandomMapItem(i, col2);
            }
            navGrid[i] = navItems;
        }
    }
    private GameObject GetMapItem(MapItem map, int row, int col)
    {
        GameObject g = Instantiate(mapItems[(int)map]);
        InitializeMapItem(ref g, row, col);
        return g;
    }

    private GameObject[] PopulateRow(int cols, int roomsToPlace,int row)
    {
        return GetLayouts(roomsToPlace,row,cols);
    }
    private GameObject[] GetLayouts(int rooms, int row,int cols)
    {
        GameObject[] layouts = new GameObject[cols];

        if (rooms <= 0)
            return layouts;

        int gap = Mathf.FloorToInt((cols - rooms) / (float)rooms);

        int span = rooms + gap * (rooms - 1);
        int offset = (cols - span) / 2;

        for (int i = 0; i < rooms; i++)
        {
            int col = offset + i * (gap + 1);
            layouts[col] = GetRandomMapItem(row, col);
        }

        return layouts;
    }
    private GameObject GetRandomMapItem(int row, int col)
    {
        int rng = Random.Range(1, 101);
        GameObject g = null;
        if (rng > 0 && rng < 60)
        {
            //enemy
            g = GetMapItem(MapItem.Normal,row,col);

        }
        else if (rng > 60 && rng < 85)
        {
            //bonus
            g = GetMapItem(MapItem.Mystery,row,col);
        }
        else
        {
            //rest
            g = GetMapItem(MapItem.Rest,row,col);
        }
        InitializeMapItem(ref g,row,col);
        return g;
    }
    private void InitializeMapItem(ref GameObject g, int row, int col) 
    {
        g.transform.SetParent(MapCanvas.transform, false);
        NavigationItem navItem = g.GetComponent<NavigationItem>();
        navItem.RowNumber = row;
        navItem.ColumnIndex = col;
        if (row != 0)
        {
            navItem.MakeConnections(ref navGrid);
        }
        if (navItem != null)
        {
            Button b = g.GetComponent<Button>();
            if (b != null)
            {
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(navItem.OnClicked);
            }
        }
        
    }
    #endregion
    private void PopulateUI()
    {
        float screenPadding = 100f;
        RectTransform mapCanvasRect = MapCanvas.GetComponent<RectTransform>();
        float canvasHeight = mapCanvasRect.rect.height;
        float canvasWidth = mapCanvasRect.rect.width;
        float actualHeight = canvasHeight - (2 * screenPadding);
        float startY = canvasHeight / 2f - screenPadding;
        for (int y = 0; y < rows; y++)
        {
            GameObject[] rowItems = navGrid[y];
            float yPos = -startY + (y * height) + 20;
            float xStart = -((rowItems.Length - 1) * width) /2f;
            for (int x = 0; x < rowItems.Length; x++)
            {
                float xPos = xStart + (x * width) + 20;
                Vector2 anchoredPos = new Vector2(xPos, yPos);
                GameObject cellObject = rowItems[x];
                if (cellObject != null)
                {
                    
                    RectTransform rt = cellObject.GetComponent<RectTransform>();
                    rt.anchoredPosition = anchoredPos;
                }
            }
        }
    }
    public void BossRoomClear()
    {

    }
    private IEnumerator DelayedUI()
    {
        yield return new WaitForEndOfFrame();
        PopulateUI();
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
            StartCoroutine(DelayedUI());
        }
    }
}
