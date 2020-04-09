using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TornadoVortex : MonoBehaviour
{
    public Transform tornadoCenter;
    public float pullSpeed;
    public float refreshRate;
    public Vector3 offset;
    private bool shouldPull;

    

    private void OnTriggerStay(Collider other)
    {
        Vector3 ForceDir = tornadoCenter.position + offset - other.transform.position;
        other.GetComponent<Rigidbody>().AddForce(ForceDir.normalized * pullSpeed * Time.deltaTime);
        Vector3.Cross(Vector3.up, ForceDir.normalized);
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
  

