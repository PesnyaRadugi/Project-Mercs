using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder
{
    private const int kStraightCost = 10;
    private const int kDiagonalCost = 14;

    
    internal List<PathNode> FindPath(PathNode start, PathNode end)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            PathNode currentPathNode = openList.OrderBy(x => x.fCost).First();
            openList.Remove(currentPathNode);
            closedList.Add(currentPathNode);

            if (currentPathNode == end)
            {
                //quit
                return GetCompleteList(start, end);
            }
        
            var neighbourNodes = GetNeighbourNodes(currentPathNode);

            foreach (var neighbour in neighbourNodes)
            {
                //1 - условная высота прыжка, если выстраивать уровни по вертикали, можно будет нахер выпилить
                if (neighbour.isBlocked || closedList.Contains(neighbour) || Mathf.Abs(currentPathNode.gridLocation.z - neighbour.gridLocation.z) > 1)
                {
                    continue;
                }

                neighbour.gCost = GetManhattenDistance(start, neighbour);
                neighbour.hCost = GetManhattenDistance(end, neighbour);
                neighbour.previousNode = currentPathNode;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }

        }

        return new List<PathNode>();         
    }

    private int GetManhattenDistance(PathNode start, PathNode neighbour)
    {
        int xDistance = Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x);
        int yDistance = Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return kDiagonalCost * Mathf.Min(xDistance, yDistance) + kStraightCost * remaining;
    }

    private List<PathNode> GetNeighbourNodes(PathNode currentPathNode)
    {
        var map = MapManager.Instance.map;

        List<PathNode> neighbours = new List<PathNode>();

        //Чекает соседние тайлы
        //Верх
        Vector2Int locationToCheck = new Vector2Int(
            currentPathNode.gridLocation.x, 
            currentPathNode.gridLocation.y + 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //Низ
        locationToCheck = new Vector2Int(
            currentPathNode.gridLocation.x, 
            currentPathNode.gridLocation.y - 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //Прави
        locationToCheck = new Vector2Int(
            currentPathNode.gridLocation.x + 1, 
            currentPathNode.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        //Леви
        locationToCheck = new Vector2Int(
            currentPathNode.gridLocation.x - 1, 
            currentPathNode.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //Говно из жопы, надо фиксить если хотим движение по диагонали

        // locationToCheck = new Vector2Int(
        //     currentOverlayTile.gridLocation.x + 1, 
        //     currentOverlayTile.gridLocation.y + 1);

        // if (map.ContainsKey(locationToCheck))
        // {
        //     neighbours.Add(map[locationToCheck]);
        // }

        // locationToCheck = new Vector2Int(
        //     currentOverlayTile.gridLocation.x + 1, 
        //     currentOverlayTile.gridLocation.y - 1);

        // if (map.ContainsKey(locationToCheck))
        // {
        //     neighbours.Add(map[locationToCheck]);
        // }

        // locationToCheck = new Vector2Int(
        //     currentOverlayTile.gridLocation.x - 1, 
        //     currentOverlayTile.gridLocation.y + 1);

        // if (map.ContainsKey(locationToCheck))
        // {
        //     neighbours.Add(map[locationToCheck]);
        // }
        
        // locationToCheck = new Vector2Int(
        //     currentOverlayTile.gridLocation.x - 1, 
        //     currentOverlayTile.gridLocation.y - 1);

        // if (map.ContainsKey(locationToCheck))
        // {
        //     neighbours.Add(map[locationToCheck]);
        // }

        return neighbours;

    }

    private List<PathNode> GetCompleteList(PathNode start, PathNode end)
    {
        List<PathNode> completeList = new List<PathNode>();

        PathNode currentNode = end;

        while(currentNode != start)
        {
            completeList.Add(currentNode);
            currentNode = currentNode.previousNode;
        }

        completeList.Reverse();

        return completeList;
    }

}
