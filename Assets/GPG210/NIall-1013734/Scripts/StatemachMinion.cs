﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GPG220.Luca.Scripts.Unit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public class StatemachMinion : UnitBase
{
    public float UnitSpeed = 0.25f;
    private UnitLevelUp unitlvlup;
    public Vector3 target;
    private bool moving = false;
    

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

    void Start()
    {
        GetComponent<Health>().deathEvent += Die;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        currentState = States.Moving;
    }

    public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        SetTargetPosition();
        if (moving)
        {
            Move();
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, 1000000))
        {
            target = hitinfo.point;
            this.transform.LookAt(target);
            moving = true;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, UnitSpeed);
        if (transform.position == target)
        {
            moving = false;
        }
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
        currentState = States.Idle;
        Debug.Log("Idle");
    }


    void Update()
    {
      

        switch (currentState)
        {
            case States.Idle:
                // is Idle/Not moving

                break;
            case States.Moving:
                
               // transform.Translate(Vector3.forward * UnitSpeed);
                
                if (unitlvlup.Kills == 2)
                {
                    UnitSpeed = 0.35f;
                }
                
                if (unitlvlup.Kills >= 3)
                {
                    UnitSpeed = 0.30f;
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
    

}