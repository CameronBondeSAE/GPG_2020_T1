using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnNotifier : MonoBehaviour
{
    // Start is called before the first frame update

    private Bounds bounds;
    private Vector3 pos;
    private void OnEnable()
    {
        if (gameObject.GetComponent<Collider>() != null)
        {
            bounds = gameObject.GetComponent<Collider>().bounds;
            pos = transform.position;
            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
        }

    }

    private void OnDisable()
    {

            GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }
}
