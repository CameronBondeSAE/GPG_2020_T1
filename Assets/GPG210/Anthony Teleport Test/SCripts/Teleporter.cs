using System;
using DG.Tweening;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class Teleporter : UnitBase
{
    
    public int damage;

    public float tweenDuration;

    private Rigidbody rb;

    public AudioSource deathSound;


    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log("Selected!");
        transform.DOShakeScale(tweenDuration, new Vector3(1f, 0, 2f), 1, 0.2f, false).SetEase(Ease.InCubic);
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
    }

    /*public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        base.OnExecuteAction(worldPosition, g);

        if (g != null)
        {
            currentTarget = g.transform;
            
        }
        

       
        Teleporting();
    }*/


//Check Death
    private void Start()
    {
        GetComponent<Health>().deathEvent += Death;
        Initialize();
    }

    public void Death(Health health1)
    {
        Destroy(gameObject);
        deathSound.Play();
    }

    //shows text for character description


    private void OnCollisionEnter(Collision other)
    {
        // Does the other object even have a Health component?
        if (other.gameObject.GetComponent<Health>() != null)
            // Do damage
            other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
    }


    /* public void Teleporting()
     {
         Debug.Log("Teleporting Activated");
         player.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         
         //player.transform.position = new Vector3(Random.Range(-Range, Range), 1, Random.Range(-Range, Range));
         player.SetActive(false);
         Invoke("DelayTeleport", 1); //delay the teleporter by 1 second
     }
 
     public void DelayTeleport()
     {
         player.SetActive(true);
     }
 */
}