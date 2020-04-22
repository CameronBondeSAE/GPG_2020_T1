using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TornadoVortex : MonoBehaviour
{
    public Transform tornadoCenter;
    public float pullSpeed;
    public float refreshRate = 2;
    public Vector3 offset;
    private bool shouldPull;
    public AnimationCurve pullspeedCurve;
    public AnimationCurve PullCenterCurve;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(PullObject(true, other));
    }

    private void OnTriggerExit(Collider other)
    {
        
        StartCoroutine(PullObject(false,other)); //pull it in if pullable
    }

    IEnumerator PullObject(bool shouldPull, Collider other)
    {
        if (shouldPull)
        {
            pullSpeed = pullspeedCurve.Evaluate(((Time.time * 0.1f) % pullspeedCurve.length));
            
            Vector3 forceDirection = tornadoCenter.position + offset - other.transform.position;
            other.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullSpeed * Time.deltaTime);
            Vector3.Cross(Vector3.up,forceDirection.normalized);
            tornadoCenter.position = new Vector3(tornadoCenter.position.x,PullCenterCurve.Evaluate(((Time.time * 0.1f)% PullCenterCurve.length)),pullSpeed);
            yield return refreshRate;
            yield return StartCoroutine(PullObject(shouldPull, other));
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
  

