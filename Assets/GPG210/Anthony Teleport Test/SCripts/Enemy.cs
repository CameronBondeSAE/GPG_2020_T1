using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject blood;
    float speed = 10f;
    //Check Death
    void Start()
    {
        GetComponent<Health>().deathEvent += Death;
    }
    
    void Update()
    {
        
        transform.Translate(Vector3.forward * speed);
    }

    public void Death(Health health)
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
