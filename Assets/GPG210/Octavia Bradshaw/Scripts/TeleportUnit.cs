using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeleportUnit : UnitBase
{
    public override bool Selectable()
    {
        return base.Selectable();
    }

    public override bool GroupSelectable()
    {
        return base.GroupSelectable();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log("Selected");
    }

	public override void OnDeSelected()
    {
        base.OnDeSelected();
    }

    public enum States
    {
        Idle,
        Walk,
        Sprint,
        Teleport,
        Attack,
        Dead
    }

    public States currentState;

    public void Idle()
    {
        
    }
    
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
        transform.position = new Vector3(Random.Range(-10f,10f),1,Random.Range(-10f,10f));
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

        // if (GetComponent<Health>().currentHealth <= 0)
        // {
            // currentState = States.Dead;
        // }
        
        switch (currentState)
        {
            case States.Idle:
                break;
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
