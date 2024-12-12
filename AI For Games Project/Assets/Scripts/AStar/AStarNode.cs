using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public int gridX, gridY;
    public bool isWall;
    public AStarNode parent;

    public int gCost, hCost;

    public int fCost() { return gCost + hCost; }

    public AStarNode(int x,int y,bool wall)
    {
        isWall = wall;
        gridX = x; gridY = y;
    }

    //public static bool operator ==(AStarNode left, AStarNode right)
    //{
    //    return left.gridX == right.gridX && left.gridY == right.gridY;
    //}

    //public static bool operator !=(AStarNode left, AStarNode right)
    //{
    //    return left.gridX != right.gridX && left.gridY != right.gridY;
    //}
}
