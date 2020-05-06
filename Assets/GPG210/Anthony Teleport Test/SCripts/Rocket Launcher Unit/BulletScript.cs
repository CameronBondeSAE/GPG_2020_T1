using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Pathfinding.AStar.New;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    public float blastRadius;

    public float explosionForce;

    private Collider[] hitColliders;
    public RocketShot bulletClone;
    public int damage;
    private UnitBase unitBase;
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<UnitBase>())
        {
            if (unitBase != null && other.gameObject.GetComponent<UnitBase>().owner != unitBase.owner)
            {
                
                if (other.gameObject.GetComponent<Health>() != null)
                {
                    // Do damage
                    other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                    DoExplosion(other.contacts[0].point);
                }
            }
        }
        // Does the other object even have a Health component?
        if (unitBase != null && other.gameObject.GetComponent<UnitBase>() == unitBase)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DoExplosion(Vector3 explosionPoint)
    {
       
        hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius);

        foreach (Collider hitcol in hitColliders)
        {
            if (hitcol.GetComponent<Rigidbody>())
            {
                hitcol.GetComponent<Rigidbody>().isKinematic = false;
                hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,explosionPoint,blastRadius,0.2f,ForceMode.Impulse);
            }
        }
            
        
    }

   
}
