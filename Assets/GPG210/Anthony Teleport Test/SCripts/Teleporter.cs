using System;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teleporter : UnitBase
{
    public bool isTeleporting = true;
    public Transform teleportTarget;
    public GameObject player;
    public float Range = -10.0f;

    //moving variables
    public float moveSpeed;
    public Transform currentTarget;
    public int damage;

    private Rigidbody rb;
   
    


    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log("Selected!");

        
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
        
    }

    public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        base.OnExecuteAction(worldPosition, g);

        if (g != null)
        {
            currentTarget = g.transform;
            
        }
        

       
        Teleporting();
    }

    
    
//Check Death
   void Start()
   {
       GetComponent<Health>().deathEvent += Death;
       Initialize();
   }

   public void Death(Health health1)
   {
       Destroy(gameObject);
   }

   //shows text for character description
  

   private void OnCollisionEnter(Collision other)
   {
       // Does the other object even have a Health component?
       if (other.gameObject.GetComponent<Health>() != null)
       {
           // Do damage
           other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
       }
   }


    public void Teleporting()
    {
        Debug.Log("Teleporting Activated");
        if (currentTarget.transform.position != null)
        {
            player.transform.position = currentTarget.transform.position;
        }
        

        //player.transform.position = new Vector3(Random.Range(-Range, Range), 1, Random.Range(-Range, Range));
        player.SetActive(false);
        Invoke("DelayTeleport", 1); //delay the teleporter by 1 second
    }

    public void DelayTeleport()
    {
        player.SetActive(true);
    }

    
}