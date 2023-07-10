using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anode
{
    public readonly bool IsWall;
    public Vector2 WorldPos;
    public readonly int Gridx;
    public readonly int GridY;

    public int GCost;
    public int HCost;
    public Anode ParentNode;
    
    public Anode(bool nWalkable, Vector2 nWorldPos, int nGridX, int nGridY)
    {
        IsWall = nWalkable;
        WorldPos = nWorldPos;
        Gridx = nGridX;
        GridY = nGridY;

    }

    public int FCost => GCost + HCost;
}
