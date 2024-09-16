using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
[Serializable]
public struct Move
{
    public MoveDirection Direction { get; set; }
    public int2 From { get; set; }
    public int2 To { get; set; }
    public bool IsValid => Direction != MoveDirection.None;
    public Move (int2 coordinates, MoveDirection direction)
    {
        Direction = direction;
        From = coordinates;
        To = coordinates + direction switch { MoveDirection.Up => int2(0, 1), MoveDirection.Right => int2(1,0), MoveDirection.Down => int2(0,-1), _ => int2(-1,0) };
    }

}
