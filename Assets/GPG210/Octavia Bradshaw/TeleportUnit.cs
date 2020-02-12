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
        Attack,
        Dead
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

    public void Dead()
    {
        Debug.Log("Dead");
        Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Health>().currentHealth <= 0)
        {
            currentState = States.Dead;
        }
        
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
            case States.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
