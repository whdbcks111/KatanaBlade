using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anode
{
    public bool IswalkAbls;
    public Vector3 WorldPos;
    public int Gridx;
    public int GridY;

    public int GCost;
    public int HCost;
    public Anode ParentNode;
    
    public Anode(bool nWalkable, Vector3 nWorldPos, int nGridX, int nGridY)
    {
        IswalkAbls = nWalkable;
        WorldPos = nWorldPos;
        Gridx = nGridX;
        GridY = nGridY;

    }

    public int FCost
    {
        get { return GCost + HCost;  }
    }
}
