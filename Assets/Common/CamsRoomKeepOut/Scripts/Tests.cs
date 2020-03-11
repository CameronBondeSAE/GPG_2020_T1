using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Abilities;
using Mirror;
using UnityEngine;

public class Tests : NetworkManager
{
    List<SpawnPoint> spawnPoints;
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        AbilityController abilityController;
        // abilityController.

    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        // conn.identity.GetComponent<PlayerBase>()
    }
}
