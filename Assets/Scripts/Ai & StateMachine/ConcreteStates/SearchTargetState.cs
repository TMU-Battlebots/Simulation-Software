using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetState : State //Inhrerit from State class, which itself has instance of AI and StateMachine
    //Neat trick: generate overrides quick from State 'interface' by rightclick -> gen override
    //Add  constructor w this too
{
    //Bool used to check between two substates 
    //they may work better as a seperate state but too late!!
    private bool goCenter;
    private bool investigateMarker;

    

    public SearchTargetState(AI ai, StateMachine stateMachine) : base(ai, stateMachine)
    {
        //Investigation state where bot will move straight towards detected L1 sensors in order to find Target
        //If L2 triggered, switch to FollowTargetState
        //else, stay here
        //Can also move to centre of Arena
    }

    public override void EnterState()
    {
        base.EnterState();
        //ai.SetCurrentStateName("Search Target State: \n Limited tracking and reserved movement, can move to center if lose target entirely");
        ai.SetCurrentStateName(0);
        Debug.Log("Entered SearchTargetState");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit SearchTargetState");
    }

    public override void Update()
    {
        base.Update();
        /*Base function
         * Drops a marker in that cone and moves towards it
         * only exits this move function when reach cone or target within L2s.
        */
            if (ai.IsL1() == true)
            {
                goCenter = false;
                ai.PlaceMarker();
               // investigateL1 = true;
                investigateMarker = true;

            }       
        else //outside of L1, move to center of arena to achieve full vision of arena
        {
            if (ai.GetMarker() == false) //makes sure the bot isnt already investigating before going to center.
            {
                goCenter = true;
                investigateMarker = false;
            }
           
        }




        //Switch state to FollowTargetState
        if (ai.IsL2() == true)
        {
            ai.SetMarker(false); //Make sure there are no markers left when it leaves this state
            ai.StateMachine.ChangeState(ai.FollowTargetState);
        }
        else if (ai.IsIdle() == true)
        {
            ai.StateMachine.ChangeState(ai.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (goCenter == true)
        {
            ai.MoveToCenter();
        }
        if (investigateMarker == true)
        {
            ai.InvestigateMarker();
        }


    }
    //slowly approach

}
