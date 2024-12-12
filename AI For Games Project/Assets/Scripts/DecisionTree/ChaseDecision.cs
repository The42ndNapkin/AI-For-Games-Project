using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseDecision : DecisionNode
{
    private Guard guard;
    private Player player;
    private List<Vector2> chasePath;
    private int pathIndex = 0;

    //Constructor
    public ChaseDecision(Guard g, Player p)
    {
        guard = g;
        player = p;
        chasePath = guard.updateAStarGrid();
        pathIndex = 0;
    }

    public override bool Execute()
    {
        if (pathIndex >= chasePath.Count)
        {
            return false;
        }
        //continues along the path to catch the player
        Debug.Log(chasePath);
        guard.setWorldPos(chasePath[pathIndex]);
        pathIndex++;
        
        return true;
    }

}
