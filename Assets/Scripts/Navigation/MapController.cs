using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    private static MapController instance;
    public static MapController Instance { get { return instance; } }

    public Canvas MapCanvas;
    public int CurrentStage = 0;
    //all possible enemy prefabs to include in scenes.
    public GameObject[] EnemyArray;
    //increment stage count at end of stage.
    public int stageCount = 0;
    public float EnemyHealthModifier = 1f;
    public float EnemyDamageModifier = 1f;

    //2D list to hold each rows items.
    private List<List<GameObject>> LevelMap;
    //Items to place on map screen (buttons which contain either normal encounter, boss ecounter (at end), random encounters, or resting scene
    //index 0 = normal, 1 = random, 2 = rest, 3 = boss.
    [SerializeField]
    private GameObject[] mapItems;
    //placement offset to x vector 2 value
    [SerializeField]
    int xOffset;
    //placement offset to y vector 2 value
    [SerializeField]
    int yOffset;
    //start point (example x0,y0)
    [SerializeField]
    Vector2 startPosition;
    //end point (example x0,y500)
    [SerializeField]
    Vector2 endPosition;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        
    }
    private void Start()
    {
        LevelMap = new List<List<GameObject>>();
        GenerateMap();
    }
    /// <summary>
    /// Populate ui canvas with the buttons for navigation to specific 
    /// encounters.
    /// Rows will contain 1-4 different items, each item can be either a enemy, unknown or rest encounter.
    /// 
    /// </summary>
    public void GenerateMap()
    {
        int rowCount = (int)(Mathf.RoundToInt(Mathf.Abs(startPosition.y - endPosition.y)) / yOffset);
        for (int i = 0; i < rowCount; i++)
        {
            //In each row randomise items, having 1-5 different rooms in each
            int rowItems = UnityEngine.Random.Range(1, 5);
            GameObject[] rowObjects = new GameObject[rowItems];
            //initialize each rowObject item
            for (int j = 0; j < rowObjects.Count(); j++)
            {
                int index = UnityEngine.Random.Range(0, mapItems.Length);
                rowObjects[j] = mapItems[index];
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
            
            float y = startY - (yCount * yOffset);
            yCount++;
            float xStart = -((row.Count - 1) * xOffset) / 2f;
            for (int xCount = 0; xCount < row.Count; xCount++)
            {
                float x = xStart + (xCount * xOffset);
                Vector2 anchoredPos = new Vector2(x, y);

                GameObject g = Instantiate(row[xCount], MapCanvas.transform);

                // Make sure the prefab has a RectTransform
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchoredPosition = anchoredPos;
            }
        }
    }

    public void IncrementStage()
    {
        stageCount++;
        EnemyHealthModifier += 0.1f;
        EnemyDamageModifier += 0.05f;

    }
    public static void LoadMapScene()
    {
        SceneManager.LoadSceneAsync("NavigationScene");
    }

}
