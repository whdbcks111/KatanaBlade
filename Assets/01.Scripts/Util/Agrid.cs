using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
     Anode[,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX;
    private int _gridSizeY;


    private void Start()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new Anode[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                                  Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.forward *
                    (y * _nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                _grid[x, y] = new Anode(walkable, worldPoint,x , y);
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

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
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

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

    public List<Anode> Path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
        if (_grid != null)
        {
            foreach (Anode n in _grid)
            {
                Gizmos.color = (n.IswalkAbls) ? Color.white : Color.red;
                if (Path != null)
                    if (Path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.WorldPos, Vector3.one * (_nodeDiameter - .5f));
            }
        }
    }
}