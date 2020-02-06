using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StatemachMinion : MonoBehaviour
{
public enum States

{
    Moving,
    Attacking,
    Dead
}

public States currentState;


void Update()
{
    switch (currentState)
    {
        case States.Moving:
          //  print("Unit is Moving");
            break;
        case States.Attacking:
         //   print("Unit is Attacking");
            break;
        case States.Dead:
          //  print("Unit is Dead");
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}
}
