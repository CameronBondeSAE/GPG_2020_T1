﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GPG220.Luca.Scripts.Pathfinding;
using GPG220.Luca.Scripts.Unit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public class StatemachMinion : UnitBase
{
    public float UnitSpeed;
    private UnitLevelUp unitlvlup;
    public Vector3 target;
    private bool moving = false;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);


    public PathFinderController pathFinderController;

    public enum States
    {
        Idle,
        Moving,
        Dead
    }

  //  public enum States 
  //  {
        
  //      Attacking,
  //      NotAttacking
  //  }

    public States currentState;

    void Awake()
    {
        unitlvlup = GetComponent<UnitLevelUp>();
       // pathFinderController = FindObjectOfType<PathFinderPath>();
    }

    void Start()
    {
        GetComponent<Health>().deathEvent += Die;
        Initialize();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        currentState = States.Moving;
    }

    public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        SetTargetPosition();
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, 1000))
        {
            target = hitinfo.point;
            this.transform.LookAt(target);
            moving = true;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target + offset, UnitSpeed * Time.deltaTime);
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

                break;
            case States.Dead:
                // Destroys unit if State is set to Dead.
                Debug.Log(gameObject.name + " has Died.");

                Destroy(gameObject);
                break;
            
        }

        if (moving == true)
        {
            Move();
            currentState = States.Moving;
        }

        else currentState = States.Idle;
        
        
        if (unitlvlup.Kills == 2)
        {
            UnitSpeed = 5f;
        }

        if (unitlvlup.Kills >= 4)
        {
            UnitSpeed = 6f;
        }
    }

    void Die(Health health)
    {
        currentState = States.Dead;
    }
}