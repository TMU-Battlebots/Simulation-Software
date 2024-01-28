using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamAttkState : State //THIS IS NOT USED THERES A BUG FOR NOW???
{
    public RamAttkState(AI ai, StateMachine stateMachine) : base(ai, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Attack");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit Attack");
    }
    public override void Update()
    {
        base.Update();
        //Base behavior of this state: Still locked on to target
        ai.TrackingTarget();
        ai.MoveForward();

        

    }
}
