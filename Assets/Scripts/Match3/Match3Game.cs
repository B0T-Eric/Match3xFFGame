using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using static Unity.Mathematics.math;
using System;

public class Match3Game : MonoBehaviour
{
    [SerializeField]
    int2 size =  8;
    //remove a tile state from enum to limit amount of tiles dropped?
    Grid2D<TileState> grid;
    List<Match> matches;
    public List<TileDrop> DroppedTiles {  get; private set; }
    public List<int2> ClearedTileCoordinates { get; private set; }
    public bool NeedsFilling { get; private set; }
    public Move PossibleMove { get; private set; }
    public TileState this[int x, int y] => grid[x, y];
    public TileState this[int2 c] => grid[c];
    public int2 Size => size;

    private Party party;
    private void Start()
    {
        party = FindAnyObjectByType<Party>();
    }
    
    public bool TryMove(Move move)
    {
        grid.Swap(move.From, move.To);
        if (FindMatches())
        {

            return true;
        }
        grid.Swap(move.From, move.To);
        return false;
    }
    public bool HasMatches => matches.Count > 0;
    public void StartNewGame()
    {
        if (grid.IsUndefined)
        {
            grid = new(size);
            matches = new();
            ClearedTileCoordinates = new List<int2>();
            DroppedTiles = new();
        }
        FillGrid();
        PossibleMove = Move.FindMove(this);
    }
    public void DropTiles()
    {
        DroppedTiles.Clear();
        for (int x = 0; x<size.x; x++)
        {
            int holeCount = 0;
            for (int y = 0; y < size.y; y++)
            {
                if (grid[x,y] == TileState.None)
                {
                    holeCount++;
                }
                else if(holeCount > 0)
                {
                    grid[x,y - holeCount] = grid[x,y];
                    DroppedTiles.Add(new TileDrop(x,y - holeCount, holeCount));
                }
            }
            for(int h = 1; h <= holeCount; h++)
            {
                grid[x, size.y - h] = Party.Instance.AllowedTileTypes[Random.Range(0, Party.Instance.AllowedTileTypes.Count)];
                DroppedTiles.Add(new TileDrop(x, size.y - h, holeCount));
            }
        }
        NeedsFilling = false;
        if (!FindMatches())
        {
            PossibleMove = Move.FindMove(this);
        }
    }
    public void ProcessMatches()
    {
        ClearedTileCoordinates.Clear();
        for(int m = 0; m < matches.Count; m++)
        {
            Match match = matches[m];
            int2 step = match.isHorizontal ? int2(1,0) : int2(0,1);
            int2 c = match.coordinates;
            int amount = 0;
            TileState state = TileState.None;
            for (int i = 0; i < match.length; c += step, i++)
            {
                if (grid[c] != TileState.None)
                {
                    //increase damage and mana of party memeber based on the tile clear
                    state = grid[c];
                    amount++;
                    grid[c] = TileState.None;
                    ClearedTileCoordinates.Add(c);
                }
            }
            AddPotentialBasedOnTileState(state, amount);
        }
        matches.Clear();
        NeedsFilling = true;
    }
    void AddPotentialBasedOnTileState(TileState state,int amount)
    {
        switch (state)
        {
            case TileState.A:
                party.IncreasePotential(ManaType.Yellow, amount);
            break;
            case TileState.B:
                party.IncreasePotential(ManaType.Green, amount);
                break;
            case TileState.C:
                party.IncreasePotential(ManaType.Orange, amount);
                break;
            case TileState.D:
                party.IncreasePotential(ManaType.White, amount);
                break;
            case TileState.E:
                party.IncreasePotential(ManaType.Blue, amount);
                break;
            case TileState.F:
                party.IncreasePotential(ManaType.Purple, amount);
                break;
            case TileState.G:
                party.IncreasePotential(ManaType.Pink, amount);
                break;
            default: break; 

        }
    }
    bool FindMatches()
    {
        for (int y = 0; y < size.y; y++)
        {
            TileState start = grid[0,y];
            int length = 1;
            for (int x = 1; x < size.x; x++)
            {
                TileState t = grid[x,y];
                if(t == start)
                {
                    length++;
                }
                else
                {
                    if(length >= 3)
                    {
                        matches.Add(new Match(x - length, y, length, true));
                    }
                    start = t;
                    length = 1;
                }
            }
            if(length >= 3)
            {
                matches.Add(new Match(size.x - length, y, length, true));
            }
        }
        for(int x = 0; x < size.x; x++)
        {
            TileState start = grid[x, 0];
            int length = 1;
            for(int y = 1; y < size.y; y++)
            {
                TileState t = grid[x,y];
                if (t == start)
                {
                    length++;
                }
                else
                {
                    if(length >= 3)
                    {
                        matches.Add(new Match(x, y - length, length, false));
                    }
                    start = t;
                    length = 1;
                }
            }
            if(length >= 3)
            {
                matches.Add(new Match(x, size.y - length, length, false));
            }
        }
        return HasMatches;
    }
    void FillGrid()
    {
        for(int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                TileState a = TileState.None, b = TileState.None;
                int potentialMatchCount = 0;
                if(x > 1)
                {
                    a = grid[x - 1, y];
                    if (a == grid[x - 2, y])
                    {
                        potentialMatchCount = 1;
                    }
                }
                if(y > 1)
                {
                    b = grid[x, y - 1];
                    if(b == grid[x, y- 2])
                    {
                        potentialMatchCount++;
                        if(potentialMatchCount == 1)
                        {
                            a = b;
                        }
                        else if(b < a)
                        {
                            (a, b) = (b,a);
                        }
                    }
                }
                TileState t = Party.Instance.AllowedTileTypes[Random.Range(0, Party.Instance.AllowedTileTypes.Count - potentialMatchCount)];
                if (potentialMatchCount > 0 && t >= a)
                {
                    t++;
                }
                if(potentialMatchCount== 2 && t >= b)
                {
                    t++;
                }
                grid[x, y] = t;
            }
        }
    }
}
