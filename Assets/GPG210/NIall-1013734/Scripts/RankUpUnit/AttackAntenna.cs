using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class AttackAntenna : UnitBase
{
    private Transform t;
    public float distance;
    [SerializeField] private bool fire = true;
    [SerializeField] public float fireRate = 3f;
    private ParticleSystem particle;
    public int damage;
    RaycastHit hit;

    void Start()
    {
        t = GetComponent<Transform>();
    }


    void FixedUpdate()
    {
        if (fire == true)

        {
            if (Physics.Raycast(t.position, t.forward, out hit, distance))
            {

                if (GetComponent<UnitBase>()?.owner != hit.transform.GetComponent<UnitBase>()?.owner)
                {
                    if (hit.transform.GetComponent<Health>() != null)
                    {
                        // Do damage
                        hit.transform.GetComponent<Health>().ChangeHealth(-damage);

                        Debug.Log(gameObject.name + hit + "Damaged Unit");

                        transform.GetComponent<ParticleSystem>().Play();

                        fire = false;
                    }
                    
                    Debug.Log(owner);
                    Debug.Log(hit.transform.GetComponent<UnitBase>().owner);
                }
            }
        }

        else
        {
            fireRate -= 1 * Time.deltaTime;
        }

        if (fireRate <= 0f && fire == false)
        {
            fire = true;
        }

        if (fireRate <= 0f)
        {
            fireRate = 3f;
        }
    }
}