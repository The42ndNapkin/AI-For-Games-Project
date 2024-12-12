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
            //Debug.Log(currentNode.gridX + " " + currentNode.gridY);

            if (currentNode == endNode)
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
                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                    neighbor.gCost = currentNode.gCost + 1;
                    neighbor.hCost = getHeurestic(neighbor, endNode);
                    neighbor.parent = currentNode;
                }
                
            }
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
        
        return (ix + iy);
    }

    int getHeurestic(AStarNode current, AStarNode neighbor)
    {
        int ix = Mathf.Abs(current.gridX - neighbor.gridX);
        int iy = Mathf.Abs(current.gridY - neighbor.gridY);
        if(current.isWall)
        {
            ix *= 100;
            iy *= 100;
        }
        return ix*ix + iy*iy;
    }
}
