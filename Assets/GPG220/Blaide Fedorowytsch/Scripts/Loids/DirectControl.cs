using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectControl : MonoBehaviour
{

    public InputAction space;

    private Rigidbody rB;

    public float forceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rB.AddForce(transform.forward * forceMultiplier);
        
    }
    
}
