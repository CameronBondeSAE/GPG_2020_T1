using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using DG.Tweening;

public class LRStateMachine : UnitBase
{
    public float tweenDuration;
    
    public enum States
    {
        Idle,
        Attacking,
        Dead
    }


    void Start()
    {
        Initialize();
        health.deathEvent += Death;
    }


    public States currentState;

    void Update()
    {
        switch (currentState)
        {
            case States.Idle:


                break;
            case States.Attacking:


                break;
            case States.Dead:
                Destroy(gameObject);

                break;
        }
    }


    void Death(Health h)
    {
        currentState = States.Dead;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        transform.DOShakeScale(tweenDuration, new Vector3(1f, 0f, 1f), 1, 0.2f, false);
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
    }
    
    
    //TODO Idle enemy search, if enemy in radius, face and start attacking.
    //TODO Passive Partner Search, if friendly LRUnit is in radius, increase strength. 
}