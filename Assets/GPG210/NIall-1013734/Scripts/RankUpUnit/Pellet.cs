using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class Pellet : AbilityBase
{
    private float speed;
    private float lifeDuration;

    private float lifeTimer;
    public int amount;

    void Start()
    {
        lifeDuration = 1.5f;
        speed = 15f;
        lifeTimer = lifeDuration;
    }


    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<Health>().ChangeHealth(-amount);
        //  Destroy(gameObject);
    }
}