using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnNotifier : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (gameObject.GetComponent<Collider>() != null)
        {
            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(transform.position,gameObject.GetComponent<Collider>().bounds));
        }

    }

    private void OnDisable()
    {
        if (gameObject.GetComponent<Collider>() != null)
        {
            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(transform.position,gameObject.GetComponent<Collider>().bounds));
        }
    }
}
