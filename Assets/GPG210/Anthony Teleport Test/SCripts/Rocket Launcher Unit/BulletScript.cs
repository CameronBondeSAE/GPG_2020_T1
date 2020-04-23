using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float blastRadius;

    public float explosionForce;

    private Collider[] hitColliders;
    public RocketManUnitBase player;
    public RocketShot bulletClone;
    public int damage = 50;
    private void OnCollisionEnter(Collision other)
    {
        DoExplosion(other.contacts[0].point);
        Destroy(gameObject);
        // Does the other object even have a Health component?
        if (other.gameObject.GetComponent<Health>() != null)
        {
            // Do damage
            other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
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
                hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,explosionPoint,blastRadius,1,ForceMode.Impulse);
            }
            }
            
        
    }

   
}
