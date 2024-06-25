using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine //Handles state switching and holds the current state, acting as the Context
{
    //Get current state (properties)
    public State CurrentEnemyState {  get; private set; }

    //starting state
    public void Initialize(State startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.EnterState();
    }

    //Change state, calls ExitState and EnterState, which can be used in each individual state
    public void ChangeState(State newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
    }
}
