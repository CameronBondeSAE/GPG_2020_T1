using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectControl : SteeringBehaviourBase
{

    public InputAction space;
    

    public float forceMultiplier;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        rB.AddForce(transform.forward * forceMultiplier);
        
    }
    
}
