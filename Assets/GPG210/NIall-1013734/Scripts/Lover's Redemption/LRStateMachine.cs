using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEditor.UI;
using UnityEngine;

public class LRStateMachine : UnitBase
{
    public State currentState;
    
    
    public enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:

                break;
            
            case State.Moving:

                break;
            
            case State.Attacking:

                break;
            
            case State.Dead:

                break;
        }
        
    }
}
