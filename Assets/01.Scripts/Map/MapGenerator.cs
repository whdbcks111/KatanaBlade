using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    public int MapSize = 30, MapCount = 50, BossCount = 3, HealCount = 2, ShopCount = 2;
    [SerializeField] Tilemap _targetTilemap, _ladderTilemap;
    [SerializeField] StageShape _spawnShape;
    [SerializeField] StageShape[] _shapes;
    [SerializeField] TileBase _wallTile, _platformTile, _ladderTile;

    [SerializeField] BossPortal _portalPrefab;
    [SerializeField] HealArea _healAreaPrefab;
    [SerializeField] GameObject[] _bossPrefabs;
    [SerializeField] ShopNPC _shopNPCPrefab;

    private readonly List<StageShape> _bottomOpened = new(), _topOpened = new(), _leftOpened = new(), _rightOpened = new();
    private readonly Dictionary<Vector2Int, Stage> _map = new();
    private readonly HashSet<Vector2Int> _usedPositions = new();

    private Vector3 _bossRoomPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("¸Ê »ý¼º±â°¡ 2°³ ÀÌ»ó ÀÖ½À´Ï´Ù!");
            Destroy(gameObject);
        }

        foreach(var shape in _shapes)
        {
            if (shape.IsTopOpened) _topOpened.Add(shape);
            if (shape.IsBottomOpened) _bottomOpened.Add(shape);
            if (shape.IsLeftOpened) _leftOpened.Add(shape);
            if (shape.IsRightOpened) _rightOpened.Add(shape);

            shape.KeyPositionOffsets = new Vector3[shape.ShapeMap.transform.childCount];
            for(int i = 0; i < shape.ShapeMap.transform.childCount; ++i)
            {
                shape.KeyPositionOffsets[i] = shape.ShapeMap.transform.GetChild(i).localPosition;
            }
        }
        Generate();
    }

    public void Generate()
    {
        _usedPositions.Clear();
        _targetTilemap.ClearAllTiles();
        _map.Clear();

        Queue<Vector2Int> queue = new();
        CreateMapTiles(Vector2Int.zero, _spawnShape, StageType.Spawn);
        queue.Enqueue(Vector2Int.zero);
        while(queue.TryDequeue(out Vector2Int curPos))
        {
            if (!_map.ContainsKey(curPos)) continue;
            var stage = _map[curPos];
            var curShape = stage.Shape;
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

                CreateMapTiles(curPos + direction, targetShapes[UnityEngine.Random.Range(0, targetShapes.Count)], StageType.Monster);
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

        for(int i = 0; i < 5; i++)
        {
            CreateWalls(new(i + MapCount + 1, 0));
            CreateWalls(new(i + MapCount + 1, 4));
            CreateWalls(new(MapCount + 1, i));
            CreateWalls(new(MapCount + 5, i));
        }
        _bossRoomPos = _targetTilemap.CellToWorld(new Vector3Int(MapCount + 3, 1) * MapSize) + Vector3.up * 4;

        var portalRenderer = _portalPrefab.GetComponent<SpriteRenderer>();
        PlaceObjects(BossCount, 500, _portalPrefab, (portal, stage) =>
        {
            stage.Type = StageType.Boss;
            portal.transform.position += Vector3.up * portalRenderer.bounds.size.y / 2;
            portal.BossMapPos = _bossRoomPos;
            portal.BossPrefab = _bossPrefabs[UnityEngine.Random.Range(0, _bossPrefabs.Length)];
        });

        var healRenderer = _healAreaPrefab.GetComponent<SpriteRenderer>();
        PlaceObjects(HealCount, 500, _healAreaPrefab, (healArea, stage) =>
        {
            stage.Type = StageType.HealArea;
            healArea.transform.position += Vector3.up * healRenderer.bounds.size.y / 2;
            healArea.HealAmount = 50f;
        });

        var shopRenderer = _shopNPCPrefab.GetComponent<SpriteRenderer>();
        PlaceObjects(ShopCount, 500, _shopNPCPrefab, (npc, stage) =>
        {
            stage.Type = StageType.Shop;
            npc.transform.position += Vector3.up * shopRenderer.bounds.size.y / 2;
        });

        var monsterPrefabs = Resources.LoadAll<Monster>("Monsters/");
        List<Vector3> spawnablePositions = new();
        
        foreach (var entry in _map)
        {
            var pos = entry.Key;
            var stage = entry.Value;
            var shape = stage.Shape;
            var offsets = shape.KeyPositionOffsets;
            if (stage.Type == StageType.Monster)
            {
                foreach(var offset in offsets)
                {
                    spawnablePositions.Add(_targetTilemap.CellToWorld((Vector3Int)pos * MapSize) + offset);
                }
            }
        }

        int remainCount = (int)(spawnablePositions.Count * 0.4f);
        int maxCount = spawnablePositions.Count;
        foreach(var pos in spawnablePositions)
        {
            // ·£´ý »Ì±â È®·ü = ³²Àº»Ì±â¼ö / Å½»öÇØ¾ßÇÒ¼ö
            if(UnityEngine.Random.value < (float)remainCount / (maxCount--))
            {
                --remainCount;

                var monsterPrefab = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];
                var monster = Instantiate(monsterPrefab,
                    pos + Vector3.up * monsterPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2, 
                    Quaternion.identity);
            }
        }
    }

    public void PlaceObjects<T>(int count, int tryLimit, T obj, Action<T, Stage> postAction) where T : UnityEngine.Object
    {
        while (count > 0 && tryLimit-- > 0)
        {
            var entry = _map.ElementAt(UnityEngine.Random.Range(0, _map.Count));
            var pos = entry.Key;
            var stage = entry.Value;
            var shape = stage.Shape;
            var offsets = shape.KeyPositionOffsets;

            if (Mathf.Max(pos.x, pos.y) <= 2) continue;
            if (stage.Type != StageType.Monster) continue;

            if (offsets.Length > 0)
            {
                var offset = offsets[UnityEngine.Random.Range(0, offsets.Length)];
                count--;
                T spawned = Instantiate(obj, _targetTilemap.CellToWorld((Vector3Int)pos * MapSize) + offset, Quaternion.identity);
                postAction(spawned, stage);
            }
        }
    }

    public void CreateMapTiles(Vector2Int pos, StageShape shape, StageType type = StageType.Monster)
    {
        _map[pos] = new Stage() { Shape = shape, Type = type };
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

public class Stage
{
    public StageShape Shape;
    public StageType Type;
}

[Serializable]
public class StageShape
{
    public Tilemap ShapeMap;
    public bool IsTopOpened, IsBottomOpened, IsLeftOpened, IsRightOpened;
    [HideInInspector] public Vector3[] KeyPositionOffsets;
}

public enum StageType
{
    Monster, Shop, Spawn, HealArea, Boss
} 