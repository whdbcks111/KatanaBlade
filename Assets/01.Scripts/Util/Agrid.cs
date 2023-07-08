using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Anode[,] grid;

    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;


    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Anode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3)Vector2.right * gridWorldSize.x / 2 -
                                  (Vector3)Vector2.up * gridWorldSize.y / 2;
        Vector3 worldPoint;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + (Vector3)Vector2.right * (x * nodeDiameter + nodeRadius) + (Vector3)Vector2.up *
                    (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Anode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Anode> GetNeighbours(Anode node)
    {
        List<Anode> neighbours = new List<Anode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.Gridx + x;
                int checkY = node.Gridx + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Anode GetNodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Anode> Path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Anode n in grid)
            {
                Gizmos.color = (n.IsWall) ? Color.white : Color.red;
                if (Path != null)
                    if (Path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.WorldPos, Vector3.one * (nodeDiameter - .5f));
            }
        }
    }
}