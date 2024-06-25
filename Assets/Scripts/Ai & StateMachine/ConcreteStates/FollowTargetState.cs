using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetState : State //L2
{
    public FollowTargetState(AI ai, StateMachine stateMachine) : base(ai, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //ai.SetCurrentStateName("Follow Target State: \n Full target tracking");
        ai.SetCurrentStateName(1);
        //Debug.Log("Entered FollowTargetState");
    }

    public override void ExitState()
    {
        base.ExitState();
        //Debug.Log("Exit FollowTargetState");
    }

    public override void Update()
    {
        base.Update();
        //Base behavior of this state: Lock on to target
        ai.TrackingTarget();

        if (ai.IsL3() == true)
        {
            ai.StateMachine.ChangeState(ai.OffensiveState);
        }
        if (ai.IsL2() != true && ai.IsL3() != true)
        {
            ai.StateMachine.ChangeState(ai.SearchTargetState);
        }
        else if (ai.IsIdle() == true)
        {
            ai.StateMachine.ChangeState(ai.IdleState);
        }

        //Switch back if lose target
        //CHECK THIS LAST
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ai.MoveSlowly();
    }
}
