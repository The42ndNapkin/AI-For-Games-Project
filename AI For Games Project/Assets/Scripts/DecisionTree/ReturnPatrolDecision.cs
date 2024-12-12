using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPatrolDecision : DecisionNode
{
    Guard guard;
    Vector2 patrolLoc;
    List<Vector2> returnPath;
    int pathIndex = 0;

    //Constructor
    public ReturnPatrolDecision(Guard g)
    {
        guard = g;
        patrolLoc = guard.getOGPatrol();
        returnPath = guard.pathfinder.findPath(guard.getGridPos(), patrolLoc);
    }

    public override bool Execute()
    {
        if (pathIndex >= returnPath.Count)
        {
            return false;
        }
        //Moves along the path towards the original patrol location
        guard.setWorldPos(returnPath[pathIndex]);
        pathIndex++;
        return true;
    }
}
