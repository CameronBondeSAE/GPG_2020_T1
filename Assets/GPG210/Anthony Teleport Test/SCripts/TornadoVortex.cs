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

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(PullObject(other, true)); //pull it in if pullable
        
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(PullObject(other, false)); //dont pull it in if not pullable
    }

    IEnumerator PullObject(Collider other, bool shouldPull)
    {
        if (shouldPull)
        {
            Vector3 ForceDir = tornadoCenter.position - other.transform.position;
            other.GetComponent<Rigidbody>().AddForce(ForceDir.normalized * pullSpeed * Time.deltaTime);
            yield return refreshRate;
            StartCoroutine(PullObject(other, shouldPull)); //keeps checking if the object is in the tornado
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
  

}