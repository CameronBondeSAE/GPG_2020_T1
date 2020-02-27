using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to event
        GetComponent<Health>().deathEvent += Death;
    }

    public void Death(Health health)
    {
        Destroy(gameObject);
    }
}
