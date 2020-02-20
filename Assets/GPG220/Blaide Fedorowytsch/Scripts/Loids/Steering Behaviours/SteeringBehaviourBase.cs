using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviourBase : MonoBehaviour
{
    public Rigidbody rB;
    // Start is called before the first frame update
     public virtual void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
