using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class NPCRC : MonoBehaviour
{
    private Transform t;
    public float distance;
    public float turnSpeed;
    public Rigidbody rb;
    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(t.position, t.forward, out hit, distance))
        {
            rb.AddTorque(0,turnSpeed,0);
        }
    }
}
