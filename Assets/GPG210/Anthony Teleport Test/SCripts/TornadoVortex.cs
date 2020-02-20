using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TornadoVortex : MonoBehaviour
{
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
         pullObj.GetComponent<Rigidbody>().AddForce(Vector3.MoveTowards(pullObj.transform.position, this.transform.position, pullSpeed * Time.deltaTime),ForceMode.Impulse); 
            
         pullObj.transform.RotateAround(Vector3.zero, transform.up,  rotateSpeed * Time.deltaTime);
      }
   }
}
