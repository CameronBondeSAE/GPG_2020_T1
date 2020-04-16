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

    private void OnEnable()
    {
        bounds = gameObject.GetComponent<Collider>().bounds;
        pos = transform.position;
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }

    private void OnDisable()
    {
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }

    public void OnAppear()
    {
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
        obstacleTweening.Disappear();
        GlobalEvents.OnPathFindingObstacleChange(new WorldPosAndBounds(pos,bounds));
    }
}
