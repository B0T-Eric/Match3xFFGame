using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Mathematics.math;

public class Match3Skin : MonoBehaviour
{
    public static Match3Skin _instance;
    public static Match3Skin Instance { get { return _instance; } }

    [SerializeField, Range(0.1f, 1f)]
    float dragThreshold = 0.5f;

    [SerializeField]
    TextMeshProUGUI gameOverText;

    [SerializeField]
    GameObject container;
    [SerializeField]
    int movesPerTurn;
    int movesRemaining;
    public int turnCount;
    Party party;

    [SerializeField]
    Match3Game game;
    Grid2D<Tile> tiles;
    float2 tileOffset;
    [SerializeField]
    TileSwapper tileSwapper;
    float busyDuration;

    [SerializeField]
    GameObject rewardPrefab;

    [SerializeField]
    GameObject enemyPrefab;
    public Canvas enemyCanvas;

    [SerializeField]
    Tile[] tilePrefabs;

    [SerializeField, Range(0.1f, 20f)]
    float dropSpeed = 8f;
    [SerializeField, Range(0f, 10f)]
    float newDropOffset = 2f;
    public bool IsPlaying => IsBusy || game.PossibleMove.IsValid;
    public bool IsBusy => busyDuration > 0f;
    private bool resolvingMove = false;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void StartNewGame()
    {
        
        party = FindAnyObjectByType<Party>();
        party.DisplayCharactersOnGameStart();
        container.gameObject.SetActive(true);
        busyDuration = 0f;
        SpawnEnemy();
        turnCount = 1;
        NewTurn();
        gameOverText.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        game.StartNewGame();
        tileOffset = -0.5f * (float2)(game.Size - 1);
        if (tiles.IsUndefined)
        {
            tiles = new(game.Size);
        }
        else
        {
            for (int y = 0; y < tiles.SizeY; y++)
            {
                for (int x = 0; x < tiles.SizeX; x++)
                {
                    tiles[x, y].Despawn();
                    tiles[x, y] = null;
                }
            }
        }
        for (int y = 0; y < tiles.SizeY; y++)
        {
            for (int x = 0; x < tiles.SizeX; x++)
            {
                tiles[x, y] = SpawnTile(game[x, y], x, y);
            }
        }
    }
    private void SpawnEnemy()
    {
        enemyCanvas.gameObject.SetActive(true);
        GameObject enemy = Instantiate(MapController.Instance.EnemyList[MapController.Instance.CurrentStage]);
        RectTransform rect = enemy.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(-200, 0);
        rect.SetParent(enemyCanvas.transform, false);
        EnemyCharacter.Instance.Init();
    }

    private void NewTurn()
    {
        foreach(BaseCharacter character in party.members)
        {
            character.TurnStart();
        }
        EnemyCharacter.Instance.TurnStart();
        movesRemaining = movesPerTurn;
        Party.Instance.TurnText.text = $"Turn {turnCount}.";
        Party.Instance.MovesText.text = $"Moves: {movesRemaining} / {movesPerTurn}";
    }

    Tile SpawnTile(TileState t, float x, float y) => tilePrefabs[(int)t - 1].Spawn(new Vector3(x + tileOffset.x, y + tileOffset.y));
    public void DoWork()
    {
        if(busyDuration > 0f)
        {
            tileSwapper.Update();
            busyDuration -= Time.deltaTime;
            if(busyDuration > 0f) 
            {
                return;
            }
        }
        if (game.HasMatches)
        {
            ProcessMatches();
        }
        else if (game.NeedsFilling)
        {
            DropTiles();
        }
        else if (resolvingMove)
        {
            resolvingMove = false;
            DecreaseMovesRemaining();
        }
        else if(!IsPlaying)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        enemyCanvas.gameObject.SetActive(false);
        container.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);

    }

    private void DropTiles()
    {
        game.DropTiles();
        for(int i = 0; i < game.DroppedTiles.Count; i++)
        {
            TileDrop drop = game.DroppedTiles[i];
            Tile tile;
            if(drop.fromY < tiles.SizeY)
            {
                tile = tiles[drop.coordinates.x, drop.fromY];
                
            }
            else
            {
                tile = SpawnTile(game[drop.coordinates], drop.coordinates.x, drop.coordinates.y + newDropOffset);
            }
            tiles[drop.coordinates] = tile;
            busyDuration = Mathf.Max(tile.Fall(drop.coordinates.y + tileOffset.y, dropSpeed), busyDuration);
        }
        
    }

    private void ProcessMatches()
    {
        game.ProcessMatches();
        for(int i = 0; i < game.ClearedTileCoordinates.Count; i++)
        {
            int2 c = game.ClearedTileCoordinates[i];
            busyDuration = Mathf.Max(tiles[c].Disappear(),busyDuration);
            tiles[c] = null;
        }
    }

    private void DoMove(Move move)
    {
        bool success = game.TryMove(move);
        Tile a = tiles[move.From], b = tiles[move.To];
        busyDuration = tileSwapper.Swap(a, b, !success);
        if(success)
        {
            tiles[move.From] = b;
            tiles[move.To] = a;
            resolvingMove = true;
        }
    }
    public void DecreaseMovesRemaining()
    {
        movesRemaining--;
        Party.Instance.MovesText.text = $"Moves: {movesRemaining} / {movesPerTurn}";
        if (movesRemaining == 0)
        {
            //end turn
            turnCount++;
            EndTurn();
        }
    }
    private void EndTurn()
    {
        
        foreach (BaseCharacter character in party.members)
        {
            if (EnemyCharacter.Instance.currentMana > 0)
            {
                character.PreformTurn();
            }
            else
            {
                break;
            }
        }
        if(EnemyCharacter.Instance.currentMana > 0)
        {
            EnemyCharacter.Instance.PreformTurn();
            if (AllPartyMembersAlive())
            {
                foreach(BaseCharacter character in party.members)
                {
                    character.TurnEnd();
                }
                EnemyCharacter.Instance.TurnEnd();
                EnemyCharacter.Instance.LogTurnDamage();
                NewTurn();
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            Victory();
        }
    }
    private void Victory()
    {
        //turn off game, display reward screen
        foreach(BaseCharacter bc in Party.Instance.members)
        {
            bc.statusEffects.Clear();
        }
        enemyCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        EnemyCharacter.Instance.Die();
        Instantiate(rewardPrefab);

    }
    private bool AllPartyMembersAlive()
    {
        int deadCount = 0;
        foreach (BaseCharacter character in party.members)
        {
            if(character.state == CharacterState.Dead)
            {
                deadCount++;
            }
        }
        if(deadCount == party.members.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool EvaluateDrag(Vector3 start, Vector3 end)
    {
        float2 a = ScreenToTileSpace(start), b = ScreenToTileSpace(end);
        var move = new Move(
            (int2)floor(a), (b - a) switch
            {
                var d when d.x > dragThreshold => MoveDirection.Right,
                var d when d.x < -dragThreshold => MoveDirection.Left,
                var d when d.y > dragThreshold => MoveDirection.Up,
                var d when d.y < -dragThreshold => MoveDirection.Down,
                _ => MoveDirection.None
            }
        );
        if(move.IsValid && tiles.AreValidCoordinates(move.From) && tiles.AreValidCoordinates(move.To))
        {
            DoMove(move);
            return false;
        }
        return true;
    }
    float2 ScreenToTileSpace(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Vector3 p = ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
        return float2(p.x - tileOffset.x + 0.5f, p.y - tileOffset.y + 0.5f);
    }
}
