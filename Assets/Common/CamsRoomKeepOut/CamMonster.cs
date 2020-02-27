using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class CamMonster : UnitBase
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    
        // Subscribe to event
        GetComponent<Health>().deathEvent += Death;
    }

    public void Death(Health health1)
    {
        Destroy(gameObject);
    }
}
