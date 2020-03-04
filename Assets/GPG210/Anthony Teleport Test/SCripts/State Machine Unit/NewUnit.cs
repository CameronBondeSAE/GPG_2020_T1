using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class NewUnit : UnitBase
{
    
    public Transform currentTarget;
    
    public StateBase currentState;

    public StateBase idleState;
    public StateBase walkingState;
    public StateBase attackingBase;
    
    void Update()
    {
        currentState.Execute();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // HACK: Hard ref
             currentState = idleState;
        }
    }

    public void ChangeState(StateBase newState)
    {
		//Change State
    }
    

}


