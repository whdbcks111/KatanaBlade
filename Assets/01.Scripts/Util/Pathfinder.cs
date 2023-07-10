using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public static class Pathfinder
{
    public static readonly Vector3Int[] Directions = { 
        new(0, 1, 10), new(0, -1, 10), new(1, 0, 10), new(-1, 0, 10)/*,
        new(1, 1, 14), new(-1, 1, 14), new(1, -1, 14), new(-1, -1, 14)*/
    };

    public static int GetH(Vector2Int cur, Vector2Int target)
    {
        return (Math.Abs(cur.x - target.x) + Math.Abs(cur.y - target.y)) * 10;
    }

    public static Path GetMinCostPath(IEnumerable<Path> paths)
    {
        Path p = null;
        foreach(var path in paths)
        {
            if(p is null || p.G + p.H > path.G + path.H)
            {
                p = path;
            }
        }
        return p;
    }

    public static Vector2 GetNextPath(Tilemap tilemap, Vector2 cur, Vector2 target)
    {
        Vector2 next = cur;
        Stack<Path> paths = FindPath(tilemap, (Vector2Int)tilemap.WorldToCell(cur), (Vector2Int)tilemap.WorldToCell(target));

        for (int i = 0; i < 2; i++)
        {
            paths.TryPop(out Path p);
            next = p.Pos + Vector2.one / 2f;
        }

        if ((next - target).sqrMagnitude < 1) next = target;

        return next;
    }

    public static Stack<Path> FindPath(Tilemap tilemap, Vector2Int start, Vector2Int target)
    {
        Stack<Path> paths = new();
        List<Path> openedList = new()
        {
            new() { Parent = null, Pos = start, G = 0, H = GetH(start, target) }
        }, closedList = new();

        while (openedList.Count > 0)
        {
            Path minCostPath = GetMinCostPath(openedList);

            if(minCostPath.Pos == target)
            {
                while(minCostPath is not null)
                {
                    paths.Push(minCostPath);
                    minCostPath = minCostPath.Parent;
                }
                return paths;
            }

            openedList.Remove(minCostPath);
            closedList.Add(minCostPath);

            foreach(var dir in Directions)
            {
                var nextPos = minCostPath.Pos + (Vector2Int)dir;
                if (tilemap.GetTile((Vector3Int)nextPos) == null &&
                    closedList.Find(p => p.Pos == nextPos) == null &&
                    openedList.Find(p => p.Pos == nextPos) == null)
                {
                    openedList.Add(new() { 
                        Parent = minCostPath, 
                        Pos = nextPos, 
                        G = minCostPath.G + dir.z, 
                        H = GetH(nextPos, target) 
                    });
                }
            }
        }

        return paths;
    }
}

public class Path
{
    public Path Parent;
    public Vector2Int Pos;
    public int G, H;
}
