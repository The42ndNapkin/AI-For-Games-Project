using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour
{
    //Get the aStar path by calling findpath and then calling bigGrid.finalPath;
    public BigGrid bigGrid;

    //Astar algorithm to return a list of movements
    public List<Vector2> findPath(Vector2 startPos, Vector2 endPos)
    {
        AStarNode startNode = bigGrid.grid[(int)startPos.x, (int)startPos.y];
        AStarNode endNode = bigGrid.grid[(int)endPos.x, (int)endPos.y];
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedList = new HashSet<AStarNode>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            AStarNode currentNode = openList[0];

            for(int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost() < currentNode.fCost() || openList[i].fCost() == currentNode.fCost() && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                return getFinalPath(startNode, endNode);
            }

            foreach(AStarNode neighbor in bigGrid.getNeighbors(currentNode))
            {
                //Debug.Log(neighbor.gridX + " " + neighbor.gridY);
                if(neighbor.isWall || closedList.Contains(neighbor))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + getManhattenDistance(currentNode, neighbor);
                if(moveCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = moveCost;
                    neighbor.hCost = getManhattenDistance(neighbor, endNode);
                    neighbor.parent = currentNode;
                    if(!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
        foreach(AStarNode v in closedList)
        {
            Debug.Log(v.gridX + " " + v.gridY);
        }
        Debug.Log("Path not Found");
        return null;
    }

    List<Vector2> getFinalPath(AStarNode startPos, AStarNode endPos)
    {
        List<Vector2> finalPath = new List<Vector2>();
        AStarNode currentNode = endPos;

        while (currentNode != startPos)
        {
            finalPath.Add(new Vector2(currentNode.gridX, currentNode.gridY));
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();
        bigGrid.finalPath = finalPath;
        return finalPath;
    }

    int getManhattenDistance(AStarNode current, AStarNode neighbor)
    {
        int ix = Mathf.Abs(current.gridX - neighbor.gridX);
        int iy = Mathf.Abs(current.gridY - neighbor.gridY);
        if( ix > iy)
        {
            return (14 * iy + 10 * (ix - iy));
        }
        return (14 * ix + 10 * (iy - ix));
    }
}
