using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindTester : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] Transform _start, _target;
    [SerializeField] GameObject _gizmos;

    private void Update()
    {
        Stack<Path> paths;
        if (Input.GetKeyDown(KeyCode.P))
        {
            paths = Pathfinder.FindPath(_tilemap,
                (Vector2Int)_tilemap.WorldToCell(_start.position), (Vector2Int)_tilemap.WorldToCell(_target.position));

            while(paths.TryPop(out Path result))
            {
                Instantiate(_gizmos, _tilemap.CellToWorld((Vector3Int)result.Pos) + Vector3.one / 2f, Quaternion.identity);
            }
        }
    }
}
