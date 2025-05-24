using UnityEngine;
using System.Collections.Generic;


public class Node
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    public Node Parent;
    public int Index { get; set; }

    public Node(bool walkable, Vector3 worldPos, int x, int y, int index) 
    {
        IsWalkable = walkable;
        WorldPosition = worldPos;
        GridX = x;
        GridY = y;
        Index = index;
    }
}