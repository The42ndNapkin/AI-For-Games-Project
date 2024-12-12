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
        if (pathIndex >= chasePath.Count - 1)
        {
            return false;
        }
        //continues along the path to catch the player
        //Debug.Log(chasePath.Count);
        guard.setWorldPos(chasePath[pathIndex]);
        pathIndex++;
        //Debug.Log(pathIndex);
        
        return true;
    }

}
