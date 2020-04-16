﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float blastRadius;

    public float explosionForce;

    private Collider[] hitColliders;
    public DasherUnitBase player;
    public RocketShot bulletClone;
    private void OnCollisionEnter(Collision other)
    {
        DoExplosion(other.contacts[0].point);
        Destroy(gameObject);
    }

    void DoExplosion(Vector3 explosionPoint)
    {
       
        hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius);

        foreach (Collider hitcol in hitColliders)
        {
            if (hitcol.GetComponent<Rigidbody>())
            {
                hitcol.GetComponent<Rigidbody>().isKinematic = false;
                hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,explosionPoint,blastRadius,1,ForceMode.Impulse);
            }
            }
            
        
    }

   
}
