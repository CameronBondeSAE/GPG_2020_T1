using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketShot : AbilityBase
{
    public GameObject bullet;
    private float bulletSpeed;
    private Vector3 target;
    public bool isShooting;
    public Transform spawnTransform;
    public float explosionForce;


    // Start is called before the first frame update
    void Start()
    {
        abilityName = "Rocket";
        abilityDescription = "Dash towards another player doing damage";
        targetRequired = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TargetExecute(Vector3 worldPos)
    {
        GameObject bulletClone = (GameObject) Instantiate(bullet, spawnTransform.position, spawnTransform.rotation);
        bulletClone.GetComponent<Rigidbody>().AddForce(-spawnTransform.up * explosionForce,ForceMode.Impulse);
        Destroy(bulletClone,3f);
        isShooting = true;
        target = worldPos;
        
        
        
        return base.TargetExecute(worldPos);
    }
}
