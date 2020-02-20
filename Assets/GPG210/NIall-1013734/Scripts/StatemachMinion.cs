using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GPG220.Luca.Scripts.Unit;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public class StatemachMinion : UnitBase
{
    public float UnitSpeed;
    public UnitLevelUp unitlvlup;

    public enum States
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    public States currentState;

    void Awake()
    {
        unitlvlup = GetComponent<UnitLevelUp>();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        currentState = States.Idle;
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
        currentState = States.Moving;
        Debug.Log("Moving");
    }


    void Update()
    {
        if (GetComponent<Health>().currentHealth <= 0)
        {
            currentState = States.Dead;
        }

        switch (currentState)
        {
            case States.Idle:
                // is Idle/Not moving.
            transform.Translate(Vector3.forward * 0);

                break;
            case States.Moving:
                
                transform.Translate(Vector3.forward * UnitSpeed);
                
                if (unitlvlup.Kills <= 3)
                {
                    //TBA
                }
                break;

            case States.Attacking:
                //   print("Unit is Attacking");
                break;
            case States.Dead:
                // Destroys unit if State is set to Dead.
                Debug.Log( gameObject.name + "Unit is Dead");
               
                Destroy(gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }
    void Die()
    {
        currentState = States.Dead;
    }

// picks random direction to wander in every time wanderTime = 0

}