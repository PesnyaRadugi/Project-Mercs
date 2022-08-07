using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    [Header("Tiles info")]
    [SerializeField] private List<TileData> tileDatas;
    internal Dictionary<TileBase, TileData> dataFromTiles;

    [Header("GUI")]
    [SerializeField] private GameObject cursorTile;

    [Header("Pathfinding shit")]
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap anotherMap;
    [SerializeField] private TileBase tile;
    private PathFinding pathfinding;
    private Tilemap _tilemap;
    public Tilemap tilemap { get { return _tilemap; } }
    private Dictionary<Vector2Int, PathNode> _map;
    public Dictionary<Vector2Int, PathNode> map { get { return _map; } }
    private List<PathNode> path;

    private void Awake() 
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Singleton of tilemap already exists, replacing it with new...");
            Destroy(this.gameObject); //Жестко надристал полный синглтон говна
        }
        else
        {
            _instance = this;
        }

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void Start() 
    {
        GenerateGrid(floor);
        path = new List<PathNode>();
        pathfinding = new PathFinding();
    }
    
    private void Update() 
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorTile.transform.position = floor.LocalToCell(mousePosition);

        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3Int pos = floor.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //pos.z = 0;
            if(path != null && map.ContainsKey(new Vector2Int(pos.x, pos.y))) {
                path = pathfinding.FindPath(0, 0, pos.x, pos.y);
                anotherMap.ClearAllTiles();
                for(int i = 0; i < path.Count; ++i) {
                    anotherMap.SetTile(new Vector3Int(path[i].x, path[i].y, 0), tile);
                }
            }

            TileBase clickedTile = floor.GetTile(pos);
            var movementCost = dataFromTiles[clickedTile].movementCost;
            print("Walking speed on clicked tile is " + movementCost);

        }
    }

    private void GenerateGrid(Tilemap map)
    {
        _tilemap = map;
        _map = new Dictionary<Vector2Int, PathNode>();

        var bounds = _tilemap.cellBounds;
        var vi = new Vector2Int();
        for (int x = bounds.xMin; x < bounds.xMax; ++x)
        {
            for (int y = bounds.yMin; y < bounds.yMax; ++y)
            {
                vi.x = x;
                vi.y = y;
                if(_tilemap.HasTile(new Vector3Int(x, y, bounds.zMin)))
                    _map.Add(vi, new PathNode(x, y, GetTileMovementCost(new Vector2Int(x, y))));
            }
        }
    }

    internal int GetTileMovementCost(Vector2 worldPosition)
    {
        Vector3Int gridPosition = floor.WorldToCell(worldPosition);
        TileBase tile = floor.GetTile(gridPosition);
        if (tile == null)
        {
            return 1;
        }

        int movementCost = dataFromTiles[tile].movementCost; 

        return movementCost;
    }
}
