using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding
{
    private const int kStraightCost = 1;

    //private const int kDiagonalCost = 14; //Uncomment for Diagonal movement 

    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var _map = MapManager.Instance.map;
        var _tilemap = MapManager.Instance.tilemap;
        var startNode = GetNode(startX, startY);
        var endNode = GetNode(endX, endY);
        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        startNode.gScore = 0;
        startNode.hScore = CalculateDistanceScore(endNode, startNode);

        var bounds = _tilemap.cellBounds;
        var vi = new Vector2Int();
        for (int x = bounds.xMin; x < bounds.xMax; ++x)
        {
            for (int y = bounds.yMin; y < bounds.yMax; ++y)
            {
                vi.x = x;
                vi.y = y;
                if(!HasTile(x, y)) continue;
                PathNode node = _map[vi];
                node.gScore = int.MaxValue;
                node.hScore = 0;
                node.cameFrom = null;
            }
        }

        int i = 0;
        while (_openList.Count > 0)
        {
            ++i;
            if (i > 1000)
            {
                Debug.Log("INFINITE!!!!");
                return null;
            }
            PathNode currentNode = FindWithLowestScore(_openList);

            //Debug.Log(currentNode);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove (currentNode);
            _closedList.Add (currentNode);

            foreach (var neighbourNode in GetNeighbours(currentNode))
            {
                if (_closedList.Contains(neighbourNode))
                    continue;

                int tentativeGScore =
                    currentNode.gScore +
                    CalculateDistanceScore(currentNode, neighbourNode) * currentNode.movementCost;
                if (tentativeGScore < neighbourNode.gScore)
                {
                    neighbourNode.cameFrom = currentNode;
                    neighbourNode.gScore = tentativeGScore;
                    neighbourNode.hScore =
                        CalculateDistanceScore(neighbourNode, endNode);

                    if (!_openList.Contains(neighbourNode))
                        _openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add (endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFrom != null)
        {
            path.Add(currentNode.cameFrom);
            currentNode = currentNode.cameFrom;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceScore(PathNode from, PathNode to)
    {
        int xDistance = Mathf.Abs(from.x - to.x);
        int yDistance = Mathf.Abs(from.y - to.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        // var movementCost = MapManager.Instance.GetTileMovementCost(new Vector2(xDistance, yDistance));
        return
        //Uncomment for Diagonal movement 
        //kDiagonalCost * Mathf.Min(xDistance, yDistance) +
        kStraightCost * remaining;
    }

    private List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        if (HasTile(node.x, node.y - 1)) neighbours.Add(GetNode(node.x, node.y - 1));
        if (HasTile(node.x, node.y + 1)) neighbours.Add(GetNode(node.x, node.y + 1));
        if (HasTile(node.x - 1, node.y)) neighbours.Add(GetNode(node.x - 1, node.y));
        if (HasTile(node.x + 1, node.y)) neighbours.Add(GetNode(node.x + 1, node.y));

        //Uncomment for diagonal movement
        // if (HasTile(x - 1, y - 1)) neighbours.Add(GetNode(x - 1, y - 1));
        // if (HasTile(x - 1, y + 1)) neighbours.Add(GetNode(x - 1, y + 1));
        // if (HasTile(x + 1, y - 1)) neighbours.Add(GetNode(x + 1, y - 1));
        // if (HasTile(x + 1, y + 1)) neighbours.Add(GetNode(x + 1, y + 1));

        /*
        Debug.Log($"Neighbours of node at {node.x},{node.y}:");
        foreach(var n in neighbours) {
            Debug.Log($"{n.x},{n.y}");
        }
        */
        return neighbours;
    }

    private PathNode FindWithLowestScore(List<PathNode> list)
    {
        PathNode lowest = list[0];
        foreach (var node in list)
        {
            if (node.fScore < lowest.fScore) lowest = node;
        }
        return lowest;
    }
 
    private PathNode GetNode(int x, int y)
    {
        return MapManager.Instance.map[new Vector2Int(x, y)];
    }

    private bool HasTile(int x, int y)
    {
        return MapManager.Instance.map.ContainsKey(new Vector2Int(x, y));
    }
}
