using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject blood;
    //Check Death
    void Start()
    {
        GetComponent<Health>().deathEvent += Death;
    }

    public void Death(Health health)
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
