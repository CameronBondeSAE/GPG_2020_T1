using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using UnityEngine;

public class TornadoVortex : MonoBehaviour
{
   //controls the radius of tornados pull range
   public float radius = 20f;
   public float maxRadiusToPullin = 10;
   //pulls objects into the tornado  negative numbers
   public float PullInPower =  -70;
   public float maxPullin = 20;
   
   public Vector3 offset;

   private Collider[] colliders;

   private GameManager gameManager;
   

   void Awake()
   {
      gameManager = FindObjectOfType<GameManager>();
      gameManager.startGameEvent += Update;
   }
   void OnDrawGizmos()
   {
      Gizmos.DrawSphere(transform.position,radius);
   }

   void Update()
   {
      colliders = Physics.OverlapSphere(transform.position, radius);
      foreach (Collider c in colliders)
      {
         if (c.GetComponent<Rigidbody>() == null)
         {
            continue; //passes control to the next iteration
         }
         //raycast that checks the rigidbodies that collide with the tornado and pulling them in when the requirements meet
         Ray ray = new Ray(transform.position,c.transform.position - transform.position);
         RaycastHit hit;
         Physics.Raycast(ray, out hit);
         if (hit.collider.name != c.gameObject.name || hit.distance < maxPullin)
         {
            continue;
         }
         else
         {
            Rigidbody rigidbody = c.GetComponent<Rigidbody>();
            c.transform.RotateAround(Vector3.up,c.transform.position,radius);
            rigidbody.AddExplosionForce(PullInPower,transform.position,radius);
         }
         

      }
   }

   
}


    


#region First TornadoPhysics Code
    /*
      private GameObject pullObj;
      public float pullSpeed;
      public float rotateSpeed;
      
      //Ontrigger function called every frame for every collider other that is touching the trigger
      public void OnTriggerStay(Collider other)
      {
         if (other.gameObject.GetComponent<Rigidbody>())
         {
            pullObj = other.gameObject;
            
            //if the object is pullable move it towards the tornado.
            pullObj.GetComponent<Rigidbody>()
               .AddForce(
                  Vector3.MoveTowards(pullObj.transform.position, this.transform.position, pullSpeed * Time.deltaTime),
                  ForceMode.Acceleration); 
               
            pullObj.transform.RotateAround(Vector3.zero, Vector3.up,rotateSpeed * Time.deltaTime);
            
         }
      }*/
   

    #endregion
  

