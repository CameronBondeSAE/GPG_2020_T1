using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaRC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, Vector3.forward, 10f);
        
        
    }
}
