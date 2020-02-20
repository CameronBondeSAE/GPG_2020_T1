using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaRC : MonoBehaviour
{
    private Transform t;
    public float distance;
    public float turnSpeed;
    public Rigidbody rb;
    void Start()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

    }


    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(t.position, t.forward, distance))
        {
            rb.AddTorque(0, turnSpeed, 0);
            
        }
        
    }
}
