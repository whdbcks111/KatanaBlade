using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static readonly int MapSize = 30, StageCount = 50, BossCount = 3;
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
    }

    public void Generate()
    {
        _targetTilemap.ClearAllTiles();

        CreateMapTiles(Vector2Int.zero, _spawnShape);
        TryGenerateTransition(Vector2Int.zero);
    }

    public void TryGenerateTransition(Vector2Int curPos, int transitionCount = 0)
    {
        var curShape = _map[curPos];
        if (curShape is null) return;

        List<Vector2Int> allowDirections = new();
        if (curShape.IsTopOpened) allowDirections.Add(Vector2Int.up);
        if (curShape.IsBottomOpened) allowDirections.Add(Vector2Int.down);
        if (curShape.IsLeftOpened) allowDirections.Add(Vector2Int.left);
        if (curShape.IsRightOpened) allowDirections.Add(Vector2Int.right);
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
public class StageShape
{
    public Tilemap ShapeMap;
    public Transform KeyPositions;
    public bool IsTopOpened, IsBottomOpened, IsLeftOpened, IsRightOpened;
    public StageType Type;
}

public enum StageType
{
    Monster, Shop, Spawn, HealArea, Boss
} 