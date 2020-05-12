using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Goap;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

public class DestroyWallInWay : ReGoapAction<string,object>
{
    private FindNearestGold _findNearestGold;
    public float distanceBeforeDestroyingWall;
    private Rigidbody rB;
    public float forceModifier;

    private bool collidedWithWall;
    protected override void Awake()
    {
        base.Awake();
        preconditions.Set("GoldFound", true);
        effects.Set("HasPathToGold",true);
        Cost = 3;
        rB = GetComponent<Rigidbody>();
    }
    public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
    { 
        base.Run(previous, next,settings,goalState, done, fail);
        
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _findNearestGold.closestGold.transform.position) >
            distanceBeforeDestroyingWall  &&  !collidedWithWall)
        {
            Vector3 target = new Vector3(_findNearestGold.closestGold.transform.position.x,transform.position.y,_findNearestGold.closestGold.transform.position.z);
            rB.AddForce((transform.position - target)*forceModifier);
        }
        else if (collidedWithWall)
        {
            failCallback(this);  
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Wall>() && !collidedWithWall)
        {
            
            other.gameObject.GetComponent<ObstacleSpawnNotifier>().OnDisappear();
            collidedWithWall = true;
        }
    }
}
