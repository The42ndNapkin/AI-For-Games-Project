using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGrid : MonoBehaviour
{
    public AStarNode[,] grid;
    public List<Vector2> finalPath;
    private int gridHeight, gridWidth;

    public void createGrid(GenerateTiles t)
    {
        finalPath = new List<Vector2>();
        gridHeight = t.getHeight();
        gridWidth = t.getWidth();
        grid = new AStarNode[gridHeight, gridWidth];
        for(int i = 0; i < t.getHeight(); i++)
        {
            for(int j = 0; j < t.getWidth(); j++)
            {
                grid[i, j] = new AStarNode(i, j, false);
                if (t.gameBoard[i,j] == GenerateTiles.Tiles.Wall)
                {
                    grid[i, j].isWall = true;
                }
            }
        }
    }

    public List<AStarNode> getNeighbors(AStarNode centerNode)
    {
        List<AStarNode> neighbors = new List<AStarNode>();
        int xCheck, yCheck;
        //Check right
        xCheck = centerNode.gridX + 1;
        yCheck = centerNode.gridY;
        if(xCheck >= 0 && xCheck < gridHeight)
        {
            if(yCheck >= 0 && yCheck < gridWidth)
            {
                neighbors.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = centerNode.gridX - 1;
        yCheck = centerNode.gridY;
        if (xCheck >= 0 && xCheck < gridWidth)
        {
            if (yCheck >= 0 && yCheck < gridHeight)
            {
                neighbors.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = centerNode.gridX;
        yCheck = centerNode.gridY + 1;
        if (xCheck >= 0 && xCheck < gridWidth)
        {
            if (yCheck >= 0 && yCheck < gridHeight)
            {
                neighbors.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = centerNode.gridX;
        yCheck = centerNode.gridY - 1;
        if (xCheck >= 0 && xCheck < gridWidth)
        {
            if (yCheck >= 0 && yCheck < gridHeight)
            {
                neighbors.Add(grid[xCheck, yCheck]);
            }
        }
        return neighbors;
    }
}
