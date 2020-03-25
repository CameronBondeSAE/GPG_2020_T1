using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TornadoPathFinding : MonoBehaviour
{
    public GameObject[] waypoints;
    private int current = 0;
    public float speed;
    public float wpRadius = 1; //could miss center of missing gameObject so using a radius

     void Update()
     {
         if (Vector3.Distance(waypoints[current].transform.position, transform.position) < wpRadius)// check the distance between the first waypoint to the next
         {
             current = Random.Range(0, waypoints.Length); //move in a random way
             if (current >= waypoints.Length) //go back to first waypoint when at last
             {
                 current = 0;
             }
         }
         transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position,
             Time.deltaTime * speed); //move towards the next waypoint at the speed set
     }


    #region Using a RigidBody to pathfind

/*
    public Transform[] target;

    public float speed;

    private int currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != target[currentTarget].position)
        {
            
            Vector3 pos = Vector3.MoveTowards(transform.position, target[currentTarget].position,
                speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
                
        }
        else
        {
            currentTarget = (currentTarget + 1) % target.Length;
        }
    }
    */

    #endregion
}
