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
    public Tweening obstacleTweening;
    private ProceduralGrowthSystem proceduralGrowthSystem;
	Collider mainCollider;

    private void OnEnable()
    {
		mainCollider = gameObject.GetComponent<Collider>();
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
		mainCollider.enabled = true;
        obstacleTweening.Appear();
        if (gameObject.GetComponent<Collider>() != null)
        {
            bounds = gameObject.GetComponent<Collider>().bounds;
            pos = transform.position;
            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
        }
    }

    public void OnDisappear()
	{
		mainCollider.enabled = false;
        obstacleTweening.Disappear();
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }
}
