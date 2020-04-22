using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketShot : AbilityBase
{
    public GameObject bullet;
    private float bulletSpeed = 10;
    public Vector3 target;
    public bool isShooting;
    public Transform spawnTransform;
    public float explosionForce;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        abilityName = "Rocket";
        abilityDescription = "Dash towards another player doing damage";
        targetRequired = true;
    }
    

    public override bool TargetExecute(Vector3 worldPos)
    {
        GameObject bulletClone = (GameObject) Instantiate(bullet, spawnTransform.position,Quaternion.identity);
        bulletClone.GetComponent<Rigidbody>().velocity = spawnTransform.forward * bulletSpeed; //bullet goes at a certain speed
        bulletClone.GetComponent<Rigidbody>().AddForce(spawnTransform.forward * explosionForce,ForceMode.Impulse); //add a force and make it explode
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(),GetComponent<Collider>());
        target = worldPos;
       
        player.transform.LookAt(target);
        spawnTransform.LookAt(target);
        
        isShooting = true;
        Destroy(bulletClone,3f);
       
        return base.TargetExecute(worldPos);
    }
}
