using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(AI ai, StateMachine stateMachine) : base(ai, stateMachine)
    {

    }

    //Virtual: lets you override them i rmb sth similar in Java was it 
    //But doesnt force you like abstract, its optional

    //Does shit when state is first entered
    public override void EnterState() 
    {
        base.EnterState();
        ai.SetCurrentStateName(3);
        //Debug.Log("Entered IdleState");
    }

    public override void ExitState() 
    {
        base.ExitState();
        //Debug.Log("Exited IdleState");
    }
    public override void Update() 
    {
        base.Update();
        if (ai.IsIdle() == false)
        {
            ai.StateMachine.ChangeState(ai.SearchTargetState);
        }
    }
    public override void PhysicsUpdate() 
    {
        base.PhysicsUpdate();    
    }

}
