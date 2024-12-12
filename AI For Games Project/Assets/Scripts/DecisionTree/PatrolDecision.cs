using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolDecision : DecisionNode
{
    private bool patrolUp;
    private bool patrolLine;
    private int patrolLength;
    private Vector2 patrolLoc;
    private Guard guard;
    private Player player;

    //Constructor
    public PatrolDecision(bool up, int length, Vector2 location, Guard g, Player p)
    {
        patrolUp = up;
        patrolLength = length;
        patrolLoc = location;
        guard = g;
        patrolLine = true;
        player = p;
    }

    //Overridden execute function to make guard patrol
    public override bool Execute()
    {
        //Move along patrol length
        //If vertical patrol
        if(patrolUp)
        {
            //If moving up
            if(patrolLine)
            {
                if(!guard.moveUp())
                {
                    patrolLine = false;
                }
                
            }
            //If moving down
            else
            {
                if(!guard.moveDown())
                {
                    patrolLine = true;
                }
                
            }
            //Checks if guard has reached end of patrol length, if so, turns around
            if (guard.getGridPos().y <= patrolLoc.y - patrolLength - 1)
            {
                patrolLine = false;
            }
            if (guard.getGridPos().y >= patrolLoc.y + patrolLength + 1)
            {
                patrolLine = true;
            }
        }
        else
        {
            //If moving left
            if (patrolLine)
            {
                if(!guard.moveLeft())
                {
                    patrolLine = false;
                }
            }
            //If moving right
            else
            {
                if(!guard.moveRight())
                {
                    patrolLine = true;
                }
                
            }
            //Checks if guard has reached end of patrol length, if so, turns around
            if (guard.getGridPos().x <= patrolLoc.x - patrolLength - 1)
            {
                patrolLine = false;
            }
            if (guard.getGridPos().x >= patrolLoc.x + patrolLength + 1)
            {
                patrolLine = true;
            }
        }
        return true;
    }

}
