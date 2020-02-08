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
    Moving,
    Attacking,
    Dead
}

public States currentState;

void awake()
{
   unitlvlup =  GetComponent<UnitLevelUp>();
}


void Update()
{
    switch (currentState)
    {
        case States.Moving:
           print("Unit is Moving");
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
}

// picks random direction to wander in every time wanderTime = 0
    void Wander()
{
    transform.eulerAngles = new Vector3(0, Random.Range (0, 180), 0);
}
}
