using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketShot : AbilityBase
{
    public GameObject bullet;
    public RocketManUnitBase RocketManUnitBase;
    private float bulletSpeed = 10;
    public Vector3 target;
    public bool isShooting;
    public Transform spawnTransform;
    public float explosionForce;
    public Rigidbody rb;
    public AudioSource explosionSound;
    
    
    // Start is called before the first frame update
    void Start()
    {
        abilityName = "Rocket";
        abilityDescription = "Shoot a rocket at someone exploding them";
        targetRequired = true;
    }

  

    public override bool TargetExecute(Vector3 worldPos)
    {
        // if (GetComponent<UnitBase>().owner != target.GetComponent<UnitBase>().owner)
        transform.LookAt(worldPos);
        GameObject bulletClone = (GameObject) Instantiate(bullet, spawnTransform.position, spawnTransform.rotation);
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), RocketManUnitBase.GetComponent<Collider>());
        explosionSound.Play();
        bulletClone.GetComponent<Rigidbody>().velocity = spawnTransform.forward * bulletSpeed; //bullet goes at a certain speed
        bulletClone.GetComponent<Rigidbody>().AddForce(spawnTransform.forward * explosionForce,ForceMode.Impulse); //add a force and make it explode

        isShooting = true;
        Destroy(bulletClone, 3f);
        return base.TargetExecute(worldPos);
    }
}