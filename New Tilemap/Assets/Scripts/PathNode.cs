using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x;
    public int y;
    public int gScore;
    public int hScore;
    public int fScore { get { return ( gScore + hScore ) * movementCost; } }
    public int movementCost;
    public PathNode cameFrom = null;

    public PathNode(int x, int y, int movementCost, int gScore = int.MaxValue, int hScore = 0)
    {
        this.x = x;
        this.y = y;
        this.gScore = gScore;
        this.hScore = hScore;
        this.movementCost = movementCost;
    }

}