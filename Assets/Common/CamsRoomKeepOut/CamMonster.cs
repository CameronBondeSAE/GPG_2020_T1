using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMonster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to event
        GetComponent<Health>().deathEvent += Death;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
