using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum MapItem { Normal, Mystery, Rest, Boss }
public class MapController : MonoBehaviour
{
    private static MapController instance;
    public static MapController Instance { get { return instance; } }
    [SerializeField]
    private GameObject pathPrefab;
    [SerializeField]
    private GameObject mapCanvas;
    public int stageCount = 0;
    public float EnemyHealthModifier = 1f;
    public float EnemyDamageModifier = 1f;
    [SerializeField]
    private GameObject scrollView;
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
    public NavigationItem currentLocation;
    public int CurrentStage = 0;
    #region grid
    [SerializeField]
    int rows, cols, roomsPerRow;
    const int height = 64;
    const int width = 64;
    GameObject[][] navGrid;
    [SerializeField]
    float spacing;
    #endregion
    const string navScene = "NavigationScene";
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
    }
    private void SceneManager_sceneUnloaded(Scene scene)
    {
        if(scene.name.Equals(navScene))
        mapCanvas.SetActive(false);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name.Equals(navScene))
        {
            mapCanvas.SetActive(true);
            if(currentLocation != null)
            {
                currentLocation.SetCleared(true);
                ToggleMapInteraction();
            }
        }
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
            MakeGrid(rows, cols);
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
        RemoveDisconnectedItems();
        StartCoroutine(DelayedUI());
    }
    private void RemoveDisconnectedItems()
    {
        for (int i = rows - 2; i > 1; i--)
        {
            GameObject[] navRow = navGrid[i];
            for (int j = 0; j < cols; j++)
            {
                GameObject colItem = navRow[j];
                if (colItem != null)
                {
                    NavigationItem navItem = colItem.GetComponent<NavigationItem>();
                    bool hasAbove = navItem.GetAboveConnections().Count > 0;
                    bool hasBelow = navItem.GetBelowConnections().Count > 0;
                    if (!hasAbove || !hasBelow)
                    {
                        RemoveFromConnections(navItem);
                        navRow[j] = null;

                        navItem.RemoveItemFromMap();

                    }
                }

            }
        }
    }
    private void RemoveFromConnections(NavigationItem navItem)
    {
        foreach (NavigationItem item in navItem.GetBelowConnections())
        {

            item.RemoveAboveConnection(navItem);
        }
        foreach (NavigationItem item in navItem.GetAboveConnections())
        {
            item.RemoveBelowConnection(navItem);
        }

    }

    private void GenerateGrid(int rows, int cols)
    {
        navGrid = new GameObject[rows][];

        for (int i = 0; i < rows; i++)
        {
            EnemyList[i] = EnemyArray[Random.Range(0, EnemyArray.Length)];
            GameObject[] navItems = new GameObject[cols];
            if (i > 1 && i < rows - 2)
            {
                int roomsToPlace = Random.Range(2, 5);
                navItems = PopulateRow(cols, roomsToPlace, i);
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
                navItems = GetLayouts(2, i, cols);
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

    private GameObject[] PopulateRow(int cols, int roomsToPlace, int row)
    {
        return GetLayouts(roomsToPlace, row, cols);
    }
    private GameObject[] GetLayouts(int rooms, int row, int cols)
    {
        GameObject[] layouts = new GameObject[cols];

        if (rooms <= 0)
            return layouts;
        if (rooms == 2)
        {
            int col1 = (cols / 2) + 1;
            int col2 = (cols / 2) - 1;
            layouts[col1] = GetRandomMapItem(row, col1);
            layouts[col2] = GetRandomMapItem(row, col2);
            return layouts;
        }
        int rawGap = Mathf.FloorToInt((cols - rooms) / (float)rooms);
        int gap = rawGap;
        if (gap == 0 && cols >= rooms * 2 - 1)
            gap = 1;
        int span = rooms + gap * (rooms - 1);
        int offset = Mathf.FloorToInt((cols - span) / 2f);
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
            g = GetMapItem(MapItem.Normal, row, col);

        }
        else if (rng > 60 && rng < 85)
        {
            //bonus
            g = GetMapItem(MapItem.Mystery, row, col);
        }
        else
        {
            //rest
            g = GetMapItem(MapItem.Rest, row, col);
        }
        InitializeMapItem(ref g, row, col);
        return g;
    }
    private void InitializeMapItem(ref GameObject g, int row, int col)
    {
        g.transform.SetParent(scrollView.transform, false);
        NavigationItem navItem = g.GetComponent<NavigationItem>();
        navItem.RowNumber = row;
        navItem.ColumnIndex = col;
        if (row != 0)
        {
            navItem.MakeConnections(ref navGrid);
        }
        if(row == 0 && CurrentStage == 0) 
        {
            navItem.PlayerIsHere = true;
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
        float spacing = 32f;
        if (scrollView == null)
        {
            scrollView = GameObject.Find("Content");
        }
        RectTransform mapCanvasRect = scrollView.GetComponent<RectTransform>();
        float canvasHeight = mapCanvasRect.rect.height;
        float canvasWidth = mapCanvasRect.rect.width;
        float actualHeight = canvasHeight - (2 * screenPadding);
        float startY = canvasHeight / 2f - screenPadding;
        for (int y = 0; y < rows; y++)
        {
            GameObject[] rowItems = navGrid[y];
            float yPos = -startY + (y * (height + spacing));
            float xStart = -((rowItems.Length - 1) * (width + spacing)) / 2f;
            for (int x = 0; x < rowItems.Length; x++)
            {
                float xPos = xStart + (x * (width + spacing));
                Vector2 anchoredPos = new Vector2(xPos, yPos);
                GameObject cellObject = rowItems[x];
                if (cellObject != null)
                {
                    NavigationItem navItem = cellObject.GetComponent<NavigationItem>();
                    RectTransform rt = cellObject.GetComponent<RectTransform>();
                    rt.anchoredPosition = anchoredPos;
                    if (navItem.GetBelowConnectionCount() > 0)
                    {
                        CreatePaths(navItem, mapCanvasRect);
                    }
                }
            }
        }
        ToggleMapInteraction();
    }
    private void CreatePaths(NavigationItem navItem, RectTransform rect)
    {
        RectTransform origin = navItem.gameObject.GetComponent<RectTransform>();
        foreach (NavigationItem below in navItem.GetBelowConnections())
        {
            GameObject path = Instantiate(pathPrefab, rect, false);
            RectTransform pathRect = path.GetComponent<RectTransform>();

            Vector2 aPos = origin.anchoredPosition;
            Vector2 bPos = below.gameObject.GetComponent<RectTransform>().anchoredPosition;
            Vector2 delta = bPos - aPos;
            float distance = delta.magnitude;
            Vector2 mid = aPos + delta * 0.5f;
            pathRect.anchoredPosition = mid;
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            pathRect.localEulerAngles = new Vector3(0f, 0f, -angle);
        }
    }
    public void BossRoomClear()
    {

    }
    public void ToggleMapInteraction()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject g = navGrid[i][j];
                if (g == null)
                {
                    continue;
                }
                NavigationItem navItem = g.GetComponent<NavigationItem>();
                navItem.ChangeEnable();
            }
        }
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
    
    public void SetCurrentPosition(NavigationItem location)
    {
        currentLocation = location;
    }
}
