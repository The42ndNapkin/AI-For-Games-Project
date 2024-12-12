using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentPatrolDecision : DecisionNode
{
    private Guard guard;
    private int patrolLength;
    private int patrolTimes;  //How many patrols before returning to original post
    private int patrolIter; //How many patrols have been completed currently
    private bool vertical;  // true if patrol is vertical
    private bool patrolLine; //true if moving in positive direction
    private Vector2 patrolLoc;
    
    //Constructor
    public VentPatrolDecision(Guard g)
    {
        guard = g;
        patrolLength = g.getPatrolLength();
        patrolTimes = g.getPatrolLength() * 2;
        patrolIter = 0;
        vertical = g.getPatrolDirection();
        patrolLoc = g.getGridPos();
        patrolLine = true;
    }

    public override bool Execute()
    {
        //If vent patrol is finished
        if (patrolIter > patrolTimes)
        {
            return false;
        }
        //Move along patrol length
        //If vertical patrol
        if (vertical)
        {
            //If moving up
            if (patrolLine)
            {
                guard.moveUp();
            }
            //If moving down
            else
            {
                guard.moveDown();
            }
            //Checks if guard has reached end of patrol length, if so, turns around
            if (guard.getGridPos().y <= patrolLoc.y - patrolLength)
            {
                patrolLine = false;
                patrolIter++;
            }
            if (guard.getGridPos().y >= patrolLoc.y + patrolLength)
            {
                patrolLine = true;
                patrolIter++;
            }
        }
        else
        {
            //If moving left
            if (patrolLine)
            {
                guard.moveLeft();
            }
            //If moving right
            else
            {
                guard.moveRight();
            }
            //Checks if guard has reached end of patrol length, if so, turns around
            if (guard.getGridPos().x <= patrolLoc.x - patrolLength)
            {
                patrolLine = false;
                patrolIter++;
            }
            if (guard.getGridPos().x >= patrolLoc.x + patrolLength)
            {
                patrolLine = true;
                patrolIter++;
            }
        }
        
        return true;
    }
}
