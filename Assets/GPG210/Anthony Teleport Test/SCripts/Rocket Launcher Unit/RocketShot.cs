using System;
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
    public Rigidbody rb;
    public AudioSource explosionSound;
    public Camera cam;

    
    

    // Start is called before the first frame update
    void Start()
    {
        
        abilityName = "Rocket";
        abilityDescription = "Shoot a rocket at someone exploding them";
        targetRequired = true;
    }

     void Update()
     {
      
     }

    public override bool TargetExecute(Vector3 worldPos)
    {
        GameObject bulletClone = (GameObject) Instantiate(bullet, spawnTransform.position,spawnTransform.rotation);
        explosionSound.Play();
        bulletClone.GetComponent<Rigidbody>().velocity = spawnTransform.forward * bulletSpeed; //bullet goes at a certain speed
        //bulletClone.GetComponent<Rigidbody>().AddForce(spawnTransform.forward * explosionForce,ForceMode.Impulse); //add a force and make it explode
        

        //pointing towards the mouse pos

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        if (Physics.Raycast(ray, out hit)) worldPos = new Vector3(hit.point.x,transform.position.y,hit.point.z);
        {
            transform.LookAt(worldPos);
        }
       
        // player.transform.LookAt(target);
       // //spawnTransform.LookAt(target);
        
        isShooting = true;
        Destroy(bulletClone,3f);
        
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(),rb.GetComponent<Collider>());
       
        return base.TargetExecute(worldPos);
    }
}
