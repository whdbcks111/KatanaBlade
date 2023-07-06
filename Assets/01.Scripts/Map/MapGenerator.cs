using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static readonly int MapSize = 30, MapCount = 50, BossCount = 3;
    public static MapGenerator Instance;

    [SerializeField] Tilemap _targetTilemap;
    [SerializeField] StageShape _spawnShape;
    [SerializeField] StageShape[] _shapes;

    private readonly Dictionary<Vector2Int, StageShape> _map = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("맵 생성기가 2개 이상 있습니다!");
            Destroy(gameObject);
        }

        Generate();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Generate();
    }

    public void Generate()
    {
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

            int maxTransition = 1 + UnityEngine.Random.Range(0, allowDirections.Count);
            for (int i = 0; i < maxTransition && allowDirections.Count > 0; i++)
            {
                var idx = UnityEngine.Random.Range(0, allowDirections.Count);
                var direction = allowDirections[idx];
                allowDirections.RemoveAt(idx);

                CreateMapTiles(curPos + direction, _shapes[UnityEngine.Random.Range(0, _shapes.Length)]);
                queue.Enqueue(curPos + direction);
            }
        }
    }

    public void CreateMapTiles(Vector2Int pos, StageShape shape)
    {
        _map[pos] = shape;

        Vector3Int mapCenterPos = (Vector3Int)pos * MapSize;

        for(int i = -MapSize / 2; i < MapSize / 2; ++i)
        {
            for (int j = -MapSize / 2; j < MapSize / 2; ++j)
            {
                _targetTilemap.SetTile(mapCenterPos + Vector3Int.right * i + Vector3Int.up * j,
                    shape.ShapeMap.GetTile(Vector3Int.right * i + Vector3Int.up * j));
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