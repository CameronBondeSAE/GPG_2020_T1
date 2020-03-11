using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class NewUnit : MonoBehaviour
{
    public StateBase currentState;

    public StateBase idleState;
    public StateBase walkingState;
    public StateBase attackingState;
    
    void Update()
    {
       
        
        //currentState.Update();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // HACK: Hard ref
             currentState = walkingState;
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


