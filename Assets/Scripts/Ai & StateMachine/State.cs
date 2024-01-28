using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State // the Car class for each Bus, Sedan, Bike
{
    //protected: private for everything other than scripts from this State class
    //ie. each state has access to AI and StateMachine
    protected AI ai;
    protected StateMachine stateMachine;

    //Constructor
    public State(AI ai, StateMachine stateMachine)
    {
        //Debug.Log("beep");
        this.stateMachine = stateMachine;
        this.ai = ai;
        
    }

    //Virtual: lets you override them i rmb sth similar in Java was it 
    //But doesnt force you like abstract, its optional

    //Does shit when state is first entered
    public virtual void EnterState() { }

    //Does shit when state is exitted
    public virtual void ExitState() { }
    //Putin this for reading stats
    public virtual void Update() { }
    //Put in this for updating physics/movement
    public virtual void PhysicsUpdate() { }

    //Can add animation trigger event here

}
