using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TeleportUnit : MonoBehaviour
{
    
    public enum States
    {
        Walk,
        Sprint,
        Teleport,
        Attack
    }

    public States currentState;
    
    public void Walking()
    {
        Debug.Log("Walking");
    }
    
    public void Sprinting()
    {
        Debug.Log("Sprinting");
    }

    
    public void Teleporting()
    {
        Debug.Log("Teleporting");
    }

    public void Attacking()
    {
        Debug.Log("Attacking");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Walk:
                break;
            case States.Sprint:
                break;
            case States.Teleport:
                break;
            case States.Attack:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
