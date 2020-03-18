using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class NewUnit : UnitBase
{
    
    public StateBase currentState;

    //States created in other scripts
    public IdleState idleState;
    public WalkingState walkingState;
    public AttackingState attackingState;

    private void Start()
    {
        Initialize();
    }

    void Update()
    {
         currentState.Execute();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // HACK: Hard ref
            ChangeState(walkingState);
        }
    }

    public void ChangeState(StateBase newState)
    {
		//Change State
        currentState.Exit();
        newState.Enter();
        currentState = newState;
    }
    

}


