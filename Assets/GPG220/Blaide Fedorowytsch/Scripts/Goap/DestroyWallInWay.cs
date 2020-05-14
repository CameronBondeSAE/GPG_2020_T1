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
    private Rigidbody rB;
    public float forceModifier;
    private PathFindToGold pathFindToGold;
    private bool collidedWithWall;
    protected override void Awake()
    {
        base.Awake();
        preconditions.Set("GoldFound", true);
        effects.Set("HasPathToGold",true);
        rB = GetComponent<Rigidbody>();
        pathFindToGold = GetComponent<PathFindToGold>();
    }
    public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
    {
        base.Run(previous, next,settings,goalState, done, fail);
        Debug.Log("why the fuck aren't i working?");
    }

    private void FixedUpdate()
    {
        // Debug.Log("Im Updating");
		if (_findNearestGold != null)
		{
			if (_findNearestGold.closestGold != null)
			{
				Vector3 position = _findNearestGold.closestGold.transform.position;
				Vector3 target   = new Vector3(position.x,transform.position.y,position.z);
				Move(target);
			}
		}
	}

    
    void Move(Vector3 v)
    {
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        rB.AddForce((v -transform.position).normalized * forceModifier);
    }

    private void OnCollisionStay(Collision other)
    {
        if (this.enabled)
        {
            if (other.gameObject.GetComponent<Wall>())
            {

                other.gameObject.GetComponent<ObstacleSpawnNotifier>().OnDisappear();
                collidedWithWall = true;
                failCallback(this);
                pathFindToGold.Cost = 1;
            }
            
        }
    }
}
