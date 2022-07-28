using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap anotherMap;
    [SerializeField] private TileBase tile;
    
    private PathFinding pathfinding;
    
    private void Start() {
        pathfinding = new PathFinding(floor);
    }
    
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Vector3Int pos = floor.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            pos.z = 0;
            //Debug.Log(pos);
            List<PathFinding.PathNode> path = pathfinding.FindPath(0, 0, pos.x, pos.y);
            if(path != null) {
                Debug.Log("WOW");
                anotherMap.ClearAllTiles();
                for(int i = 0; i < path.Count; ++i) {
                    anotherMap.SetTile(new Vector3Int(path[i].x, path[i].y, 0), tile);
                }
            }
        }
    }
}
