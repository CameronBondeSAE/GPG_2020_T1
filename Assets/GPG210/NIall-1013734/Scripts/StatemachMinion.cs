using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class StatemachMinion : MonoBehaviour
{
    public float wanderTime;
    public float wanderSpeed;
    public UnitLevelUp unitlvlup;

    public enum States
    {
        Wander,
        Moving,
        Attacking,
        Dead
    }

    public States currentState;

    void Awake()
    {
        unitlvlup = GetComponent<UnitLevelUp>();
    }


    void Update()
    {
        switch (currentState)
        {
            case States.Wander:
                print("Unit is Wandering");
                // random wandering if state is set to Moving.
                if (wanderTime > 0)
                {
                    transform.Translate(Vector3.forward * wanderSpeed);
                    wanderTime -= Time.deltaTime;
                }
                else
                {
                    wanderTime = Random.Range(0.2f, 0.6f);
                    wanderSpeed = Random.Range(0f, 0.3f);
                    Wander();
                }

                break;
            case States.Moving:
                if (unitlvlup.Kills <= 3)
                {
                    // increase Unit speed/attack by unitLevelup Kills divided by 6.
                }
                break;

            case States.Attacking:
                //   print("Unit is Attacking");
                break;
            case States.Dead:
                // Destroys unit if State is set to Dead.
                print("Unit is Dead");

                Destroy(gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (GetComponent<Health>().currentHealth <= 0)
        {
            currentState = States.Dead;
        }


    }

// picks random direction to wander in every time wanderTime = 0
    void Wander()
    {
        transform.eulerAngles = new Vector3(0, Random.Range(0, 359), 0);
    }
}