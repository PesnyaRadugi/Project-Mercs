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
    [SerializeField] internal Tilemap floor;
    [SerializeField] internal Tilemap anotherMap;
    [SerializeField] private TileBase tile;
    private PathFinding pathfinding;
    private Tilemap _tilemap;
    public Tilemap tilemap { get { return _tilemap; } }
    private Dictionary<Vector2Int, PathNode> _map;
    public Dictionary<Vector2Int, PathNode> map { get { return _map; } }
    internal List<PathNode> path;
    [SerializeField] private CharacterController character;

    private void Awake() 
    {
        //Жестко надристал полный синглтон говна
        if (_instance != null && _instance != this)
        {
            Debug.Log("Singleton of tilemap already exists, replacing it with new...");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // Обращение к Scriptable object
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
        // Отображение курсора
        var mousePosition = floor.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if (map.ContainsKey((Vector2Int)mousePosition))
        {
            cursorTile.transform.position = floor.LocalToCell(mousePosition);
        }

        //Построение и отрисовка пути
        if(Input.GetMouseButtonDown(0)) 
        {
            var pos = floor.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(path != null && map.ContainsKey((Vector2Int)pos)) {
                path = pathfinding.FindPath(character.pos.x, character.pos.y, pos.x, pos.y);
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
                    _map.Add(vi, new PathNode(x, y, GetTileMovementCost(vi)));
            }
        }
    }

    internal int GetTileMovementCost(Vector2 tilePositionInGrid)
    {
        Vector3Int gridPosition = floor.WorldToCell(tilePositionInGrid);
        TileBase tile = floor.GetTile(gridPosition);
        if (tile == null)
        {
            return 1;
        }

        int movementCost = dataFromTiles[tile].movementCost; 

        return movementCost;
    }
}
