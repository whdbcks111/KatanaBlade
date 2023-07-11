using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static readonly int MapSize = 30, MapCount = 50, BossCount = 3;
    public static MapGenerator Instance;

    [SerializeField] Tilemap _targetTilemap, _ladderTilemap;
    [SerializeField] StageShape _spawnShape;
    [SerializeField] StageShape[] _shapes;
    [SerializeField] TileBase _wallTile, _platformTile, _ladderTile;

    [SerializeField] BossPortal _portalPrefab;

    private readonly List<StageShape> _bottomOpened = new(), _topOpened = new(), _leftOpened = new(), _rightOpened = new();

    private readonly Dictionary<Vector2Int, StageShape> _map = new();

    private readonly HashSet<Vector2Int> _usedPositions = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("맵 생성기가 2개 이상 있습니다!");
            Destroy(gameObject);
        }

        foreach(var shape in _shapes)
        {
            if (shape.IsTopOpened) _topOpened.Add(shape);
            if (shape.IsBottomOpened) _bottomOpened.Add(shape);
            if (shape.IsLeftOpened) _leftOpened.Add(shape);
            if (shape.IsRightOpened) _rightOpened.Add(shape);
        }
        Generate();
    }

    public void Generate()
    {
        _usedPositions.Clear();
        _targetTilemap.ClearAllTiles();
        _map.Clear();

        Queue<Vector2Int> queue = new();
        CreateMapTiles(Vector2Int.zero, _spawnShape);
        queue.Enqueue(Vector2Int.zero);
        while(queue.TryDequeue(out Vector2Int curPos))
        {
            if (!_map.ContainsKey(curPos)) continue;
            var curShape = _map[curPos];
            if (_map.Count >= MapCount) break;

            List<Vector2Int> allowDirections = new();

            if (curShape.IsTopOpened && !_map.ContainsKey(curPos + Vector2Int.up)) 
                allowDirections.Add(Vector2Int.up);
            if (curShape.IsBottomOpened && !_map.ContainsKey(curPos + Vector2Int.down)) 
                allowDirections.Add(Vector2Int.down);
            if (curShape.IsLeftOpened && !_map.ContainsKey(curPos + Vector2Int.left)) 
                allowDirections.Add(Vector2Int.left);
            if (curShape.IsRightOpened && !_map.ContainsKey(curPos + Vector2Int.right)) 
                allowDirections.Add(Vector2Int.right);

            for (int i = 0; allowDirections.Count > 0; i++)
            {
                var idx = UnityEngine.Random.Range(0, allowDirections.Count);
                var direction = allowDirections[idx];
                allowDirections.RemoveAt(idx);

                List<StageShape> targetShapes = null;
                if (direction.x > 0) targetShapes = _leftOpened;
                else if (direction.x < 0) targetShapes = _rightOpened;
                else if (direction.y > 0) targetShapes = _bottomOpened;
                else if (direction.y < 0) targetShapes = _topOpened;

                if (targetShapes is null || targetShapes.Count == 0) continue;

                CreateMapTiles(curPos + direction, targetShapes[UnityEngine.Random.Range(0, targetShapes.Count)]);
                queue.Enqueue(curPos + direction);
            } 
        }

        foreach(var pos in _map.Keys)
        {
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0) CreateWalls(pos + new Vector2Int(i, j));
                }
            }
        }
    }

    public List<Vector2> GetProperPoints(Vector2Int size, Vector2Int pos)
    {
        List<Vector2> result = new();

        Vector2Int mapCenterPos = pos * MapSize;
        var shape = _map[pos];

        for (int i = -MapSize / 2; i < MapSize / 2 - size.x + 1; ++i)
        {
            for (int j = -MapSize / 2; j < MapSize / 2 - size.y + 1; ++j)
            {
                bool flag = true;
                
                for(int sx = 0; sx < size.x; ++sx)
                {
                    for(int sy = 0; sy < size.y; ++sy)
                    {
                        var offset = Vector3Int.right * (i + sx) + Vector3Int.up * (j + sy);
                        var hasTile = shape.ShapeMap.HasTile(offset);

                        if (hasTile)
                        {
                            flag = false;
                            break;
                        }
                    }
                }

                if(flag)
                {
                    result.Add(_targetTilemap.CellToWorld((Vector3Int)(mapCenterPos + (size - Vector2Int.one) / 2)));
                }

            }
        }

        return result;
    }

    public void CreateMapTiles(Vector2Int pos, StageShape shape)
    {
        _map[pos] = shape;
        _usedPositions.Add(pos);

        Vector3Int mapCenterPos = (Vector3Int)pos * MapSize;

        for(int i = -MapSize / 2; i < MapSize / 2; ++i)
        {
            for (int j = -MapSize / 2; j < MapSize / 2; ++j)
            {
                var offset = Vector3Int.right * i + Vector3Int.up * j;

                Tilemap tilemap = _targetTilemap;
                TileBase targetTile;
                var tile = shape.ShapeMap.GetTile(offset);

                if (tile == _ladderTile)
                {
                    targetTile = _ladderTile;
                    tilemap = _ladderTilemap;
                }
                else if (tile is null) targetTile = null;
                else targetTile = _platformTile;

                tilemap.SetTile(mapCenterPos + offset, targetTile);

            }
        } 
    }

    public void CreateWalls(Vector2Int pos)
    {
        if (_usedPositions.Contains(pos)) return;
        _usedPositions.Add(pos);
        Vector3Int mapCenterPos = (Vector3Int)pos * MapSize;

        for (int i = -MapSize / 2; i < MapSize / 2; ++i)
        {
            for (int j = -MapSize / 2; j < MapSize / 2; ++j)
            {
                var p = mapCenterPos + Vector3Int.right * i + Vector3Int.up * j;
                _targetTilemap.SetTile(p, _wallTile);
            }
        }
    }
}

[Serializable]
public struct StageShape
{
    public Tilemap ShapeMap;
    public bool IsTopOpened, IsBottomOpened, IsLeftOpened, IsRightOpened;
    public StageType Type;
}

public enum StageType
{
    Monster, Shop, Spawn, HealArea, Boss
} 