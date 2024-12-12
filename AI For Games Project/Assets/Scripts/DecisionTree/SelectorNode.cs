using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : DecisionNode
{
    private List<DecisionNode> nodes;
    private Guard guard;
    private Player player;
    private DecisionNode previousAction;

    public SelectorNode(Guard g, Player p)
    {
        guard = g;
        player = p;
        //Starts guard patrolling by default
        previousAction = new PatrolDecision(g.getPatrolDirection(), g.getPatrolLength(), g.getPatrolLoc(), g, p);
        nodes = new List<DecisionNode>();
        nodes.Add(previousAction);
    }

    //This is the function that does the full guard AI decision tree
    public override bool Execute()
    {
        //Do we know where the player is?
        if(guard.doesKnowPlayer())
        {
            //Was the guard already chasing?
            if(previousAction.GetType() == typeof(ChaseDecision))
            {
                //If chase is done, start searching
                if(!previousAction.Execute())
                {
                    //If the guard can still see player, keep chasing
                    if(guard.canSeePlayer())
                    {
                        previousAction = new ChaseDecision(guard, player);
                        previousAction.Execute();
                    }
                    else
                    {
                        //Otherwise keep searching
                        //turn off noise alert
                        guard.noiseOff();
                        //Start new search action
                        previousAction = new SearchDecision(guard, player);
                        previousAction.Execute();
                    }
                }
            }
            else
            {
                //start new chase action
                previousAction = new ChaseDecision(guard, player);
                previousAction.Execute();
            }
        }
        //were we just chasing the player?
        else if(previousAction.GetType() == typeof(ChaseDecision))
        {
            //Is the guard done chasing?
            if (!previousAction.Execute())
            {
                //turn off noise alert
                guard.noiseOff();
                //Start new search action
                previousAction = new SearchDecision(guard, player);
                previousAction.Execute();
            }
        }
        //Were we checking nearby obstacles?
        else if(previousAction.GetType() == typeof(SearchDecision))
        {
            //Have we finished checking nearby obstacles?
            if(!previousAction.Execute())
            {
                //Is there a vent nearby?
                if(previousAction.isVent())
                {
                    //Start patrolling the vent
                    previousAction = new VentPatrolDecision(guard);
                    previousAction.Execute();
                }
                //If not, return to original post
                else
                {
                    previousAction = new ReturnPatrolDecision(guard);
                    previousAction.Execute();
                }
            }
        }
        //Are we in the middle of a vent patrol?
        else if(previousAction.GetType() == typeof(VentPatrolDecision))
        {
            //Has our vent patrol completed?
            if(!previousAction.Execute())
            {
                //if so, start returning to original patrol location
                previousAction = new ReturnPatrolDecision(guard);
                previousAction.Execute();
            }
        }
        //are we returning to our original location?
        else if(previousAction.GetType() == typeof(ReturnPatrolDecision))
        {
            //have we returned to our original patrol location?
            if(!previousAction.Execute())
            {
                previousAction = new PatrolDecision(guard.getPatrolDirection(), guard.getPatrolLength(), guard.getPatrolLoc(), guard, player);
                previousAction.Execute();
            }
        }
        else
        {
            previousAction.Execute();
        }
        Debug.Log(previousAction.GetType());
        return true;
    }
}
