using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchDecision : DecisionNode
{
    private Guard guard;
    private Player player;
    private Vector2 centerGridLoc;
    private List<Vector2> spacesToCheck;
    private List<Vector2> pathToSpace;

    //Constructor
    public SearchDecision(Guard g, Player p)
    {
        guard = g;
        player = p;
        centerGridLoc = g.getGridPos();
        spacesToCheck = getSpacesInSight();
        pathToSpace = new List<Vector2>();
    }

    public override bool Execute()
    {
        pathfindToNextCheck();
        if (spacesToCheck.Count <= 0)
        {
            return false;
        }
        guard.setWorldPos(pathToSpace[0]);
        if(guard.getGridPos() == spacesToCheck[0])
        {
            spacesToCheck.RemoveAt(0);
        }
        return true;
    }

    //updates astar path for looking at next obstacle
    void pathfindToNextCheck()
    {
        pathToSpace = guard.pathfinder.findPath(guard.getGridPos(), spacesToCheck[0]);
    }

    //Gets all the spaces in the 2-block guard sightline
    List<Vector2> getSpacesInSight()
    {
        List<Vector2> l = new List<Vector2>();
        for(int i = -2; i < 2; i++)
        {
            for(int j = -2; j < 2; j++)
            {
                if(guard.tiles.gameBoard[(int)centerGridLoc.x + i, (int)centerGridLoc.y + j] == GenerateTiles.Tiles.Water || guard.tiles.gameBoard[(int)centerGridLoc.x + i, (int)centerGridLoc.y + j] == GenerateTiles.Tiles.Box)
                {
                    l.Add(new Vector2((int)centerGridLoc.x + i, (int)centerGridLoc.y + j));
                }
            }
        }
        return l;
    }

    //Returns true if a vent is within the 2-block guard sightline of the last known player position
    public override bool isVent()
    {
        List<Vector2> l = new List<Vector2>();
        for (int i = -2; i < 2; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                if (guard.tiles.gameBoard[(int)centerGridLoc.x + i, (int)centerGridLoc.y + j] == GenerateTiles.Tiles.Vent)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
