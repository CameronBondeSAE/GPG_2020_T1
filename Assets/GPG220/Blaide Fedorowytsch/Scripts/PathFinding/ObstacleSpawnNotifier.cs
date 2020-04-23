using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using UnityEngine;

public class ObstacleSpawnNotifier : MonoBehaviour
{
    // Start is called before the first frame update

    private Bounds bounds;
    private Vector3 pos;
    public Wall obstacleWall;
    private ProceduralGrowthSystem proceduralGrowthSystem;
	// Collider mainCollider;

    private void OnEnable()
    {
		// mainCollider = gameObject.GetComponent<Collider>();
		// bounds = mainCollider.bounds;
  //       pos = transform.position;
  //       GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }

    // private void OnDisable()
    // {
        // GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    // }

    public void OnAppear()
	{
        obstacleWall.Appear();
        if (obstacleWall.col != null)
        {
            bounds = obstacleWall.col.bounds;
            pos = transform.position;
            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
        }
    }

    public void OnDisappear()
	{
		// mainCollider.enabled = false;
        obstacleWall.Disappear();
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }
}
