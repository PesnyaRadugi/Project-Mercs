using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder
{
    internal List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();
            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                //quit
                return GetCompleteList(start, end);
            }
        
            var neighbourTiles = GetNeighbourTiles(currentOverlayTile);

            foreach (var neighbour in neighbourTiles)
            {
                //1 - условная высота прыжка, если выстраивать уровни по вертикали, можно будет нахер выпилить
                if (neighbour.isBlocked || closedList.Contains(neighbour) || Mathf.Abs(currentOverlayTile.gridLocation.z - neighbour.gridLocation.z) > 1)
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);
                neighbour.previousTile = currentOverlayTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }

        }

        return new List<OverlayTile>();         
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    private List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //Чекает соседние тайлы
        //Верх
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x, 
            currentOverlayTile.gridLocation.y + 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //Низ
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x, 
            currentOverlayTile.gridLocation.y - 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //Прави
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1, 
            currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        //Леви
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1, 
            currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;

    }

    private List<OverlayTile> GetCompleteList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> completeList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while(currentTile != start)
        {
            completeList.Add(currentTile);
            currentTile = currentTile.previousTile;
        }

        completeList.Reverse();

        return completeList;
    }

}
