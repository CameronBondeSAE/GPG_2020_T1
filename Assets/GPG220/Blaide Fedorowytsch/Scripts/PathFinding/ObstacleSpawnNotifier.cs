using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObstacleSpawnNotifier : MonoBehaviour
{
    // Start is called before the first frame update

    private Bounds bounds;
    private Vector3 pos;
    public Wall obstacleWall;
    private ProceduralGrowthSystem proceduralGrowthSystem;
    public LayerMask killLayer;
	// Collider mainCollider;

   // private void OnEnable()
    //{
		// mainCollider = gameObject.GetComponent<Collider>();
		// bounds = mainCollider.bounds;
  //       pos = transform.position;
  //       GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
   // }

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


        foreach (Collider collider in  Physics.OverlapBox(transform.position, new Vector3(1, 1, 2.5f), transform.rotation, killLayer))
        {
			Health h = collider.GetComponent<Health>(); 
			// h?.ChangeHealth(- h.CurrentHealth * 2);
			h?.InstaKill();
		}
    }

    [Button(ButtonSizes.Small)]
    public void OnDisappear()
	{
		// mainCollider.enabled = false;
        obstacleWall.Disappear();
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }
}
