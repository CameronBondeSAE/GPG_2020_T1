using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Loids;
using UnityEngine;

public class Alignment : SteeringBehaviourBase
{
    public Neighbour neighbour;
    public AnimationCurve rotationCurve;
    public override void Start()
    {
        neighbour = GetComponent<Neighbour>();
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 eulersAvg = neighbour.averageOfNeighbourRotations().eulerAngles;
        Vector3 angTorque =
            eulersAvg * rotationCurve.Evaluate( Vector3.Angle(eulersAvg,transform.rotation.eulerAngles));

        angTorque = angTorque * -(0.9f * rB.angularVelocity.magnitude);
       // rB.AddTorque(angTorque);
    }
}
